using System;
using System.Web.UI;

namespace Havit.CastleWindsor.WebForms.Example
{
    public partial class DefaultPage : Page
    {
        private readonly IMyDependecy myDependecy;

        public DefaultPage(IMyDependecy myDependecy)
        {
            this.myDependecy = myDependecy;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            litHello.Text = myDependecy.MyMethod();
        }
    }
}