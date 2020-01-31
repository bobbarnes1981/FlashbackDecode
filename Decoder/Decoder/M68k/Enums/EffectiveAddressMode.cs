namespace Decoder.M68k.Enums
{
    /// <summary>
    /// Effective address mode enum.
    /// </summary>
    public enum EffectiveAddressMode
    {
        /// <summary>
        /// Data register Dn
        /// </summary>
        DataRegister = 0x00, // 0000 0000

        /// <summary>
        /// Address register An
        /// </summary>
        AddressRegister = 0x08, // 0000 1000

        /// <summary>
        /// Address (An)
        /// </summary>
        Address = 0x10, // 0001 0000

        /// <summary>
        /// Address post increment (An)+
        /// </summary>
        AddressPostIncrement = 0x18, // 0001 1000

        /// <summary>
        /// Address pre decrement -(An)
        /// </summary>
        AddressPreDecrement = 0x20, // 0010 0000

        /// <summary>
        /// Address with displacement (d16, An)
        /// </summary>
        AddressWithDisplacement = 0x28, // 0010 1000

        /// <summary>
        /// Address with index (d8, An, Xn)
        /// </summary>
        AddressWithIndex = 0x30, // 0011 0000

        /// <summary>
        /// Absoulte word (xxx).W
        /// </summary>
        AbsoluteWord = 0x38, // 0011 1000

        /// <summary>
        /// Absolute long (xxx).L
        /// </summary>
        AbsoluteLong = 0x39, // 0011 1001

        /// <summary>
        /// Program counter with displacement (d16, PC)
        /// </summary>
        ProgramCounterWithDisplacement = 0x3A, // 0011 1010

        /// <summary>
        /// Program counter with index (d8, PC, Xn)
        /// </summary>
        ProgramCounterWithIndex = 0x3B, // 0011 1011

        /// <summary>
        /// Immediate #imm
        /// </summary>
        Immediate = 0x3C, // 0011 1100
    }
}
