using System;

namespace AntMe.English
{
    /// <summary>
    /// Attribute to request access to the user interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class UserinterfaceAccessAttribute : AccessAttribute { }
}