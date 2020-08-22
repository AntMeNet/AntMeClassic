using System;

namespace AntMe.SharedComponents.AntVideo.Block
{
    /// <summary>
    /// Possible Fields of sugarupdate
    /// </summary>
    [Flags]
    internal enum SugarFields
    {
        Amount = 1,
        Range = 2
    }

    internal sealed class SugarUpdate : UpdateBase
    {
        #region Updateinformation

        private int m_aMenge;
        private int m_aRadius;

        #endregion

        public SugarUpdate() { }

        // Blocklayout:
        // ...
        // ushort aAmount
        // ushort aRadius

        public SugarUpdate(Serializer serializer)
            : base(serializer)
        {
            if (HasChanged(SugarFields.Amount))
            {
                m_aMenge = serializer.ReadUShort();
            }
            if (HasChanged(SugarFields.Range))
            {
                m_aRadius = serializer.ReadUShort();
            }
        }

        public override void Serialize(Serializer serializer)
        {
            base.Serialize(serializer);

            if (HasChanged(SugarFields.Amount))
            {
                serializer.SendUshort((ushort)m_aMenge);
            }
            if (HasChanged(SugarFields.Range))
            {
                serializer.SendUshort((ushort)m_aRadius);
            }
        }

        public void Change(SugarFields field)
        {
            Change((int)field);
        }

        public bool HasChanged(SugarFields field)
        {
            return HasChanged((int)field);
        }

        #region Properties

        /// <summary>
        /// Gets or sets the absolute value of amount.
        /// </summary>
        public int AbsoluteAmount
        {
            get { return m_aMenge; }
            set { m_aMenge = value; }
        }

        /// <summary>
        /// Gets or sets the absolute value for radius.
        /// </summary>
        public int AbsoluteRadius
        {
            get { return m_aRadius; }
            set { m_aRadius = value; }
        }

        #endregion
    }
}