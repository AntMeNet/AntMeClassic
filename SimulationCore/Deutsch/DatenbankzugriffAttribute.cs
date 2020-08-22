using System;

namespace AntMe.Deutsch
{
    /// <summary>
    /// Attribut zur Anfrage von Zugriffen auf Datenbanken.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DatenbankzugriffAttribute : ZugriffAttribute { }
}