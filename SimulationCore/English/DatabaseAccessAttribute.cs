using System;

namespace AntMe.English
{
    /// <summary>
    /// Attribute to request access to database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DatabaseAccessAttribute : AccessAttribute { }
}