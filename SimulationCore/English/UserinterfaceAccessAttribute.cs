using System;

namespace AntMe.English {
    /// <summary>
    /// Attribute to request access to the userinterface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class UserinterfaceAccessAttribute : AccessAttribute {}
}