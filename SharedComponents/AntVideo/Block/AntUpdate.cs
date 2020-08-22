using AntMe.SharedComponents.States;
using System;

namespace AntMe.SharedComponents.AntVideo.Block
{
    /// <summary>
    /// Liste der Felder, die sich durch ein Update ändern könnten
    /// </summary>
    [Flags]
    internal enum AntFields
    {
        TargetType = 1,
        PositionX = 2,
        PositionY = 4,
        Direction = 8,
        TargetPositionX = 16,
        TargetPositionY = 32,
        Load = 64,
        Vitality = 128,
        LoadType = 256,
        DebugMessage = 512,
    };

    internal sealed class AntUpdate : UpdateBase
    {
        #region Updateinformation

        public int aLoad;
        public LoadType aLoadType = LoadType.None;
        public int aTargetPositionX;
        public int aTargetPositionY;
        public TargetType aTargetType = TargetType.None;
        public int aVitality;
        public string aDebugMessage;
        public int dDirection;
        public int dPositionX;
        public int dPositionY;

        #endregion

        public AntUpdate() { }

        // Blocklayout:
        // - ...
        // - byte TargetType
        // - sbyte PositionX
        // - sbyte PositionY
        // - short Direction
        // - ushort Vitality
        // - ushort TargetPositionX
        // - ushort TargetPositionY
        // - byte Load
        // - byte LoadType

        public AntUpdate(Serializer serializer)
            : base(serializer)
        {
            if (HasChanged(AntFields.TargetType))
            {
                aTargetType = (TargetType)serializer.ReadByte();
            }
            if (HasChanged(AntFields.PositionX))
            {
                dPositionX = serializer.ReadSByte();
            }
            if (HasChanged(AntFields.PositionY))
            {
                dPositionY = serializer.ReadSByte();
            }
            if (HasChanged(AntFields.Direction))
            {
                dDirection = serializer.ReadShort();
            }
            if (HasChanged(AntFields.Vitality))
            {
                aVitality = serializer.ReadUShort();
            }
            if (HasChanged(AntFields.TargetPositionX))
            {
                aTargetPositionX = serializer.ReadUShort();
            }
            if (HasChanged(AntFields.TargetPositionY))
            {
                aTargetPositionY = serializer.ReadUShort();
            }
            if (HasChanged(AntFields.Load))
            {
                aLoad = serializer.ReadByte();
            }
            if (HasChanged(AntFields.LoadType))
            {
                aLoadType = (LoadType)serializer.ReadByte();
            }
            if (HasChanged(AntFields.DebugMessage))
            {
                aDebugMessage = serializer.ReadString();
            }
        }

        public override void Serialize(Serializer serializer)
        {
            base.Serialize(serializer);

            if (HasChanged(AntFields.TargetType))
            {
                serializer.SendByte((byte)aTargetType);
            }
            if (HasChanged(AntFields.PositionX))
            {
                serializer.SendSByte((sbyte)dPositionX);
            }
            if (HasChanged(AntFields.PositionY))
            {
                serializer.SendSByte((sbyte)dPositionY);
            }
            if (HasChanged(AntFields.Direction))
            {
                serializer.SendShort((short)dDirection);
            }
            if (HasChanged(AntFields.Vitality))
            {
                serializer.SendUshort((ushort)aVitality);
            }
            if (HasChanged(AntFields.TargetPositionX))
            {
                serializer.SendUshort((ushort)aTargetPositionX);
            }
            if (HasChanged(AntFields.TargetPositionY))
            {
                serializer.SendUshort((ushort)aTargetPositionY);
            }
            if (HasChanged(AntFields.Load))
            {
                serializer.SendByte((byte)aLoad);
            }
            if (HasChanged(AntFields.LoadType))
            {
                serializer.SendByte((byte)aLoadType);
            }
            if (HasChanged(AntFields.DebugMessage))
            {
                serializer.SendString(aDebugMessage);
            }
        }

        public void Change(AntFields field)
        {
            Change((int)field);
        }

        public bool HasChanged(AntFields field)
        {
            return HasChanged((int)field);
        }
    }
}