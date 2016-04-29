using System;

namespace AntMe.English {
    /// <summary>
    /// Attribute to request access to network.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class NetworkAccessAttribute : AccessAttribute {}
}