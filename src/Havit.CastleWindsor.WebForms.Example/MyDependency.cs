namespace Havit.CastleWindsor.WebForms.Example
{
    public class MyDependency : IMyDependecy
    {
        public string SayHello()
        {
            return "Hello from a dependency!";
        }
    }
}
