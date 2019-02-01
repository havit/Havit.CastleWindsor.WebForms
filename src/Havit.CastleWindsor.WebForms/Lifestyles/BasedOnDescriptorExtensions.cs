using System;
using System.Collections.Generic;
using System.Text;
using Castle.MicroKernel.Registration;

namespace Havit.CastleWindsor.WebForms.Lifestyles
{
	/// <summary>
	/// Extension methods for BasedOnDescriptor.
	/// </summary>
	public static class BasedOnDescriptorExtensions
    {
		/// <summary>
		/// Hybrid lifestyle - per web request or per scope.
		/// </summary>
		public static BasedOnDescriptor LifestyleHybridPerWebRequestScope(this BasedOnDescriptor basedOnDescriptor)
        {
            return basedOnDescriptor.Configure(c => c.LifestyleHybridPerWebRequestScope());            
        }
    }
}
