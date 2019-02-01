using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Registration.Lifestyle;
using System;

namespace Havit.CastleWindsor.WebForms.Lifestyles
{
	/// <summary>
	/// Extension methods for ComponentRegistration&lt;TService&gt;.
	/// </summary>
	public static class ComponentRegistrationExtensions
    {
		/// <summary>
		/// Hybrid lifestyle - per web request or per scope.
		/// </summary>
		public static ComponentRegistration<TService> LifestyleHybridPerWebRequestScope<TService>(this ComponentRegistration<TService> componentRegistration)
            where TService : class
        {
            return componentRegistration.LifeStyle.HybridPerWebRequestScope<TService>();
        }
    }

}
