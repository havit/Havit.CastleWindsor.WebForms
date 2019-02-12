using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Havit.CastleWindsor.WebForms.Example
{
	public partial class DemoControl : System.Web.UI.UserControl
	{
		private readonly IMyDependecy myDependecy;

		public DemoControl(IMyDependecy myDependecy)
		{
			this.myDependecy = myDependecy;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			DemoLiteral.Text = myDependecy.SayHello();
		}
	}
}