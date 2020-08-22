using System;

namespace AntMe.SharedComponents.AntVideo.Block
{
    [Flags]
    internal enum FruitFields
    {
        PositionX = 1,
        PositionY = 2,
        CarringAnts = 4
    };

    internal sealed class FruitUpdate : UpdateBase
    {
        public byte aCarringAnts;
        public int dPositionX;
        public int dPositionY;

        public FruitUpdate() { }

        // Blocklayout:
        // ...
        // - sbyte PositionX
        // - sbyte PositionY
        // - byte CarringAnts

        public FruitUpdate(Serializer serializer)
            : base(serializer)
        {

            if (HasChanged(FruitFields.PositionX))
            {
                dPositionX = serializer.ReadSByte();
            }

            if (HasChanged(FruitFields.PositionY))
            {
                dPositionY = serializer.ReadSByte();
            }

            if (HasChanged(FruitFields.CarringAnts))
            {
                aCarringAnts = serializer.ReadByte();
            }
        }

        public override void Serialize(Serializer serializer)
        {
            base.Serialize(serializer);

            if (HasChanged(FruitFields.PositionX))
            {
                serializer.SendSByte((sbyte)dPositionX);
            }

            if (HasChanged(FruitFields.PositionY))
            {
                serializer.SendSByte((sbyte)dPositionY);
            }

            if (HasChanged(FruitFields.CarringAnts))
            {
                serializer.SendByte(aCarringAnts);
            }
        }

        public void Change(FruitFields field)
        {
            Change((int)field);
        }

        public bool HasChanged(FruitFields field)
        {
            return HasChanged((int)field);
        }
    }
}