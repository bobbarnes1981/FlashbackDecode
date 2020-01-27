namespace Decoder
{
    public enum EffectiveAddressMode
    {
        DataRegister                = 0x00, // 0000 0000
        AddressRegister             = 0x08, // 0000 1000
        Address                     = 0x10, // 0001 0000
        Address_PostIncremenet      = 0x18, // 0001 1000
        Address_PreDecrement        = 0x20, // 0010 0000
        AddressWithDisplacement     = 0x28, // 0010 1000
        AddressWithIndex            = 0x30, // 0011 0000

        AbsoluteWord                = 0x38, // 0011 1000
        AbsoluteLong                = 0x39, // 0011 1001

        ProgramCounter_Displacement = 0x3A, // 0011 1010
        ProgramCounter_Index        = 0x3B, // 0011 1011

        Immediate                   = 0x3C, // 0011 1100
    }
}
