using System;
using System.Web.UI;

namespace Havit.CastleWindsor.WebForms.Example
{
    public partial class _Default : Page
    {
        private readonly IMyDependecy myDependecy;

        public _Default(IMyDependecy myDependecy)
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