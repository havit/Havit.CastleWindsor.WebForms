using System;

namespace Havit.CastleWindsor.WebForms
{
    /// <summary>
    /// Marks properties that shall be injected in Web Services
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class InjectAttribute : Attribute
	{
	}
}
