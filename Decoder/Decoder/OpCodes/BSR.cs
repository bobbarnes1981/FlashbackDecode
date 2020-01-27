namespace Decoder.OpCodes
{
    class BSR : OpCode
    {
        protected override string definition => "01100001bbbbbbbb";

        public override string Name => "BSR";

        public override string Description => "Branch to Subroutine";

        public override string Operation => "SP-4 -> SP; PC -> (SP); PC+dn -> PC";

        public override string Syntax => string.Format("{0} <label>", Name);

        public override string Assembly
        {
            get
            {
                switch (Size)
                {
                    case Size.Byte:
                        return string.Format("{0} {1}", FullName, (sbyte)EA);
                    case Size.Word:
                        return string.Format("{0} {1}", FullName, (short)EA);
                    default:
                        throw new InvalidStateException();
                }
            }
        }

        public BSR(MachineState state)
            : base(state)
        {
            if (Size == Size.Word)
            {
                EA = readEA(EffectiveAddressMode.Immediate, 0x00);
                state.PC -= 2; // remove auto increment
            }

            state.PC += EA;

            throw new System.NotImplementedException();
        }

        protected override Size getSize()
        {
            byte displacement = (byte)getImmediate();

            if (displacement == 0x00)
            {
                return Size.Word;
            }
            else
            {
                EA = (sbyte)displacement;
                return Size.Byte;
            }
        }
    }
}
