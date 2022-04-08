namespace AntMe.Plugin.Fna
{
    /// <summary>
    /// class, to hold selection-information.
    /// </summary>
    internal struct Selection
    {
        /// <summary>
        /// Gets or sets the selected item or null, if empty
        /// </summary>
        public object Item;

        /// <summary>
        /// Gets or sets the type of selected item.
        /// </summary>
        public SelectionType SelectionType;

        /// <summary>
        /// Gets or sets additional information to the selected item.
        /// </summary>
        public string AdditionalInfo;
    }
}