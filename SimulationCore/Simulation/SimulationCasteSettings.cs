using System;
using System.Configuration;

namespace AntMe.Simulation
{
    /// <summary>
    /// Holds the set of caste-Settings.
    /// </summary>
    [Serializable]
    public struct SimulationCasteSettings
    {
        /// <summary>
        /// Offset to shift the array-index.
        /// </summary>
        public int Offset;

        /// <summary>
        /// Sum of all points.
        /// </summary>
        public int Sum;

        /// <summary>
        /// List of caste-setting-columns.
        /// </summary>
        public SimulationCasteSettingsColumn[] Columns;

        /// <summary>
        /// Gives the lowest Column-Index.
        /// </summary>
        public int MinIndex
        {
            get { return Offset; }
        }

        /// <summary>
        /// Gives the highest Column-Index.
        /// </summary>
        public int MaxIndex
        {
            get { return Offset + Columns.Length - 1; }
        }

        /// <summary>
        /// Delivers the right caste-column.
        /// </summary>
        /// <param name="index">index of column</param>
        /// <returns>caste-Column</returns>
        public SimulationCasteSettingsColumn this[int index]
        {
            get
            {
                if (index < Offset)
                {
                    throw new IndexOutOfRangeException(Resource.SimulationCoreSettingsCasteColumnToSmall);
                }
                else if (index > MaxIndex)
                {
                    throw new IndexOutOfRangeException(Resource.SimulationCoreSettingsCasteColumnToBig);
                }

                // Deliver the right column
                return Columns[index - Offset];
            }
        }

        /// <summary>
        /// Checks the value-ranges of all properties.
        /// </summary>
        public void RuleCheck()
        {

            if (Offset > 0)
            {
                throw new ConfigurationErrorsException("Ein Kasten-Offset darf nicht größer als 0 sein");
            }

            foreach (SimulationCasteSettingsColumn column in Columns)
            {
                column.RuleCheck();
            }
        }
    }
}