using Castle.MicroKernel.Registration;
using System;
using System.Web;

namespace Havit.CastleWindsor.WebForms.Example
{
    public class Global : HttpApplication
    {
        public void Application_Start(object sender, EventArgs e)
        {
           var container = this.AddWindsorContainer();
            container.Register(Component.For<IMyDependecy>().ImplementedBy<MyDependency>());
        }
    }
}