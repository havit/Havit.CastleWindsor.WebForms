using Castle.MicroKernel.ModelBuilder.Inspectors;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;

namespace Havit.CastleWindsor.WebForms
{
	/// <summary>
	/// The Castle Windsor adapter for WebObjectActivator.
	/// </summary>
	internal class ContainerServiceProvider : IServiceProvider, IRegisteredObject
	{
		public IWindsorContainer Container { get; }

		internal IServiceProvider NextServiceProvider { get; }

		private readonly ConcurrentDictionary<Type, bool> typesToCreateByActivator = new ConcurrentDictionary<Type, bool>(); // there is no ConcurrentHashSet in .NET FW.
		private readonly ConcurrentDictionary<Type, bool> typesToCreateByWindsorContainer = new ConcurrentDictionary<Type, bool>(); // there is no ConcurrentHashSet in .NET FW.
		private readonly ConcurrentDictionary<Type, bool> typesToCreateByNextServiceProvider = new ConcurrentDictionary<Type, bool>(); // there is no ConcurrentHashSet in .NET FW.

		public ContainerServiceProvider(IServiceProvider next)
		{
			var windsorContainer = new WindsorContainer();
			// We don't want to inject properties, only ctors
			windsorContainer.Kernel.ComponentModelBuilder.RemoveContributor(windsorContainer.Kernel.ComponentModelBuilder.Contributors.OfType<PropertiesDependenciesModelInspector>().Single());

			Container = windsorContainer;
			NextServiceProvider = next;
			HostingEnvironment.RegisterObject(this);
		}

		/// <summary>
		/// Implementation of IServiceProvider. Asp.net will call this method to
		/// create the instances of Page/UserControl/HttpModule etc. 
		/// Unfortunatelly not WebServices (.asmx)
		/// </summary>
		public object GetService(Type serviceType)
		{
			// Performance:
			// duration(WindsorContainer.Resolve+Release) = 4x duration(Activator.CreateInstance)
			if (typesToCreateByActivator.ContainsKey(serviceType)) // >90%
			{
				return CreateInstanceByActivator(serviceType);
			}

			if (typesToCreateByWindsorContainer.ContainsKey(serviceType)) // < 10%
			{
				return CreateInstanceByWindsorContainer(serviceType);
			}

			if (typesToCreateByNextServiceProvider.ContainsKey(serviceType)) // 0%
			{
				return CreateInstanceByNextServiceProvider(serviceType);
			}

			// we continue only for types which was never before resolved

			// We must register dynamically compiled resources (pages, controls, master pages, handlers ...)
			// lock protects repeated registration to WindsorContainer in case of parallel requests
			lock (serviceType)
			{
				if (ShouldBeRegisteredToWindsorContainer(serviceType)
					&& !Container.Kernel.HasComponent(serviceType)) // protects repeated registration to WindsorContainer in case of parallel requests
				{
					// Lifestyle is *Transient* 
					// If it would be PerWebRequest, we couldn't use the same control on one page twice - resolved would be only the first, and the second would be reused)
					// NamedAutomaticaly with serviceType.AssemblyQualifiedName - enables multiple compilations of a page during development, every compilation is in a new assembly
					Container.Register(Component.For(serviceType).ImplementedBy(serviceType).LifestyleTransient().NamedAutomatically(serviceType.AssemblyQualifiedName));
				}
			}

			object result = null;

			// If we have component registered, we will resolve the service
			if (Container.Kernel.HasComponent(serviceType))
			{
				result = CreateInstanceByWindsorContainer(serviceType);
				typesToCreateByWindsorContainer.TryAdd(serviceType, true);

			}

			// Try the next provider if we don't have result
			if ((result == null) && (NextServiceProvider != null) && (result = CreateInstanceByNextServiceProvider(serviceType)) != null)
			{				
				typesToCreateByNextServiceProvider.TryAdd(serviceType, true);
			}

			// Default activation
			if ((result == null) && (result = CreateInstanceByActivator(serviceType)) != null)
			{
				// Remember it
				typesToCreateByActivator.TryAdd(serviceType, true);
			}

			return result;
		}

		/// <summary>
		/// Stop of registration.
		/// </summary>
		public void Stop(bool immediate)
		{
			HostingEnvironment.UnregisterObject(this);

			Container.Dispose();
		}


		private bool ShouldBeRegisteredToWindsorContainer(Type serviceType)
		{
			return ((typeof(Control).IsAssignableFrom(serviceType) // User controls (.ascx), Master Pages (.master) and custom controls inherit from Control class
				|| typeof(IHttpHandler).IsAssignableFrom(serviceType)) // Generic handlers (.ashx) and also pages (.aspx) implements IHttpHandler
				&& (serviceType.GetConstructor(Type.EmptyTypes) == null)); // Performance for controls (LiteralControl, Labels, ...): When there is parameterless constructor, Castle Windsor is not required
		}

		/// <summary>
		/// Creates serticeType instance by Activator.
		/// </summary>
		private object CreateInstanceByActivator(Type serviceType)
		{
			return Activator.CreateInstance(
				serviceType,
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.CreateInstance,
				null,
				null,
				null);
		}

		/// <summary>
		/// Creates serticeType instance by Windsor Container.
		/// </summary>
		private object CreateInstanceByWindsorContainer(Type serviceType)
		{
			object instance = Container.Resolve(serviceType);
			// And because transient, we must release component on end request - else we would make memory leaks
			HttpContext.Current.AddOnRequestCompleted(_ => Container.Release(instance));

			return instance;
		}

		/// <summary>
		/// Creates serticeType instance by the next service provider.
		/// </summary>
		private object CreateInstanceByNextServiceProvider(Type serviceType)
		{
			return NextServiceProvider.GetService(serviceType);
		}
	}
}
