namespace AntMe.SharedComponents.AntVideo
{
    /// <summary>
    /// List of possible block types in stream.
    /// </summary>
    internal enum BlockType
    {
        // Frame-blocks
        StreamStart = 0x01,
        StreamEnd = 0x02,
        FrameStart = 0x03,
        FrameEnd = 0x04,

        // Start-blocks
        Frame = 0x10,
        Colony = 0x11,
        Anthill = 0x12,
        Ant = 0x13,
        Marker = 0x14,
        Fruit = 0x15,
        Caste = 0x16,
        Sugar = 0x17,
        Bug = 0x18,
        Team = 0x19,

        // Update-blocks
        FrameUpdate = 0x20,
        ColonyUpdate = 0x21,
        AnthillUpdate = 0x22,
        AntUpdate = 0x23,
        MarkerUpdate = 0x24,
        FruitUpdate = 0x25,
        CasteUpdate = 0x26,
        SugarUpdate = 0x27,
        BugUpdate = 0x28,
        TeamUpdate = 0x29,

        // Lost-blocks
        FrameLost = 0x30,
        ColonyLost = 0x31,
        AnthillLost = 0x32,
        AntLost = 0x33,
        MarkerLost = 0x34,
        FruitLost = 0x35,
        CasteLost = 0x36,
        SugarLost = 0x37,
        BugLost = 0x38,
        TeamLost = 0x39
    }
}