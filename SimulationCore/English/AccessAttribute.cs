using System;

namespace AntMe.English {
    /// <summary>
    /// Baseattribute for all access-requesting attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class AccessAttribute : Attribute {
        /// <summary>
        /// Short description of what the ant will do with the requested rights.
        /// </summary>
        public string Description;
    }
}