using System;

namespace AntMe.Deutsch
{
    /// <summary>
    /// Attribut zur Anfrage von Dateizugriffen.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DateizugriffAttribute : ZugriffAttribute { }
}