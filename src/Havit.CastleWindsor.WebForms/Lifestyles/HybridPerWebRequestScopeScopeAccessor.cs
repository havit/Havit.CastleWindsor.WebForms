using System;
using System.Reflection;
using System.Web;
using Castle.Facilities.AspNet.SystemWeb;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace Havit.CastleWindsor.WebForms.Lifestyles
{
    /// <summary>
    /// Scope accessor for a hybrid lifestyle - per web request or per scope.
    /// </summary>
    public class HybridPerWebRequestScopeScopeAccessor : IScopeAccessor
    {
        private readonly IScopeAccessor webRequestScopeAccessor = new WebRequestScopeAccessor();
        private readonly IScopeAccessor lifetimeScopeAccessor = new LifetimeScopeAccessor();

        /// <inheritdoc />
        public ILifetimeScope GetScope(CreationContext context)
        {
            if (HttpContext.Current != null && IsWebRequestModuleInitialized())
            {
                return webRequestScopeAccessor.GetScope(context);
            }

            return lifetimeScopeAccessor.GetScope(context);
        }

        private bool disposed;

        /// <summary>
        /// Disposes scope accessor.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    webRequestScopeAccessor.Dispose();
                    lifetimeScopeAccessor.Dispose();
                }
                disposed = true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~HybridPerWebRequestScopeScopeAccessor()
        {
            Dispose(false);
        }

        /// <summary>
        /// Method checks if <see cref="PerWebRequestLifestyleModule" /> is initialized because of situations, where 
        /// HttpContext.Current exists, but .Request does not. For example in Application_Start in global.asax
        /// </summary>
        private static bool IsWebRequestModuleInitialized()
        {
            var fieldInfo = typeof(PerWebRequestLifestyleModule).GetField("initialized", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField);
            return (bool)fieldInfo.GetValue(null);
        }
    }
}
