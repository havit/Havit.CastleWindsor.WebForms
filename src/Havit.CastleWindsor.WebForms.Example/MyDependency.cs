namespace Havit.CastleWindsor.WebForms.Example
{
    public class MyDependency : IMyDependecy
    {
        public string MyMethod()
        {
            return "Hello from a dependency!";
        }
    }
}
