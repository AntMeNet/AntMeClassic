using System;

namespace AntMe.SharedComponents.Plugin
{
    /// <summary>
    /// Attribute to mark a plugin as important that should be selected at the 
    /// first start of AntMe. This attribute only takes effect, if there is no 
    /// configuration-file from earlier starts.
    /// </summary>
    /// <author>Tom Wendel (tom@antme.net)</author>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PreselectedAttribute : Attribute { }
}