namespace Decoder.M68k.OpCodes
{
    //class BSR : OpCode
    //{
    //    protected override string definition => "01100001bbbbbbbb";

    //    public override string Name => "BSR";

    //    public override string Description => "Branch to Subroutine";

    //    public override string Operation => "SP-4 -> SP; PC -> (SP); PC+dn -> PC";

    //    public override string Syntax => string.Format("{0} <label>", Name);

    //    public override string Assembly
    //    {
    //        get
    //        {
    //            switch (Size)
    //            {
    //                case Size.Byte:
    //                    return string.Format("{0} {1}", FullName, (sbyte)EA);
    //                case Size.Word:
    //                    return string.Format("{0} {1}", FullName, (short)EA);
    //                default:
    //                    throw new InvalidStateException();
    //            }
    //        }
    //    }

    //    public BSR(MachineState state)
    //        : base(state)
    //    {
    //        if (Size == Size.Word)
    //        {
    //            EA = readEA(EffectiveAddressMode.Immediate, 0x00);
    //        }

    //        state.SP -= 4;
    //        state.Write(state.SP + 0, (byte)((state.PC >> 24) & 0xFF));
    //        state.Write(state.SP + 1, (byte)((state.PC >> 16) & 0xFF));
    //        state.Write(state.SP + 2, (byte)((state.PC >> 8) & 0xFF));
    //        state.Write(state.SP + 3, (byte)((state.PC >> 0) & 0xFF));

    //        if (Size == Size.Word)
    //        {
    //            state.PC += EA - 2;
    //        }
    //        else
    //        {
    //            state.PC += EA;
    //        }
    //    }

    //    protected override Size getSize()
    //    {
    //        byte displacement = (byte)getImmediate();

    //        if (displacement == 0x00)
    //        {
    //            return Size.Word;
    //        }
    //        else
    //        {
    //            EA = displacement;
    //            return Size.Byte;
    //        }
    //    }
    //}
}
