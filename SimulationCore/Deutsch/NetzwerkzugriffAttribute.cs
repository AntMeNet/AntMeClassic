using System;

namespace AntMe.Deutsch
{
    /// <summary>
    /// Attribut zur Anfrage von Netzwerkzugriff.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class NetzwerkzugriffAttribute : ZugriffAttribute { }
}