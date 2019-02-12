using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Havit.CastleWindsor.WebForms.Example
{
	[WebService]
	public class WebService : InjectableWebServiceBase
	{
		[Inject]
		public IMyDependecy MyDependecy { get; set; }

		[WebMethod]
		public string SayHello()
		{
			return MyDependecy.SayHello();
		}
	}
}
