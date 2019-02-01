using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Registration.Lifestyle;
using System;

namespace Havit.CastleWindsor.WebForms.Lifestyles
{
    /// <summary>
    /// Extension methods for LifestyleGroup&lt;TService&gt;.
    /// </summary>
    public static class LifestyleGroupExtensions
    {
        /// <summary>
        /// Hybrid lifestyle - per web request or per scope.
        /// </summary>
        public static ComponentRegistration<TService> HybridPerWebRequestScope<TService>(this LifestyleGroup<TService> lifestyleGroup)
            where TService : class
        {			
            return lifestyleGroup.Scoped<HybridPerWebRequestScopeScopeAccessor>();
        }
    }

}
