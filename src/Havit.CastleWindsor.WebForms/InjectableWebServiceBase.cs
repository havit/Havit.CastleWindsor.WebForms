using System.Web.Services;

namespace Havit.CastleWindsor.WebForms
{
    /// <summary>
    /// Abstract base class for ASMX Web Services. It can have injected properties throght [Inject] attribute
    /// </summary>
    public abstract class InjectableWebServiceBase : WebService
	{
		/// <summary>
        /// Constructor takes care of resolving Web Service dependencies
		/// </summary>
		protected InjectableWebServiceBase()
		{
			DependencyInjectionWebFormsHelper.InitializeInstance(this);
		}

		/// <summary>
        /// We must release Web Service dependencies
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				DependencyInjectionWebFormsHelper.ReleaseDependencies(this);
			}
		}
	}
}