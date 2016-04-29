using System;

namespace AntMe.English {
    /// <summary>
    /// Attribute to request fileaccess.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class FileAccessAttribute : AccessAttribute {}
}