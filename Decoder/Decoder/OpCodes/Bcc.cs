namespace Decoder.OpCodes
{
    public class Bcc : OpCode
    {
        public override string Name => "B";

        public override string Description => "Branch Conditionally";

        public override string Operation => "If CONDITION TRUE PC+dn -> PC";

        public override string Syntax => $"{Name}{getCondition()} <label>";

        public override string Assembly
        {
            get
            {
                switch (Size)
                {
                    case Size.Byte:
                        return $"{Name}{getCondition()} {(sbyte)EA}";
                    case Size.Word:
                        return $"{Name}{getCondition()} {(short)EA}";
                    default:
                        throw new InvalidStateException();
                }
            }
        }

        protected override string definition => "0110ccccbbbbbbbb";

        public Bcc(MachineState state)
            : base(state)
        {
            if (Size == Size.Word)
            {
                EA = readEA(EffectiveAddressMode.Immediate, 0x00);
            }

            if (checkCondition(EA))
            {
                if (Size == Size.Word)
                {
                    state.PC += EA - 2;
                }
                else
                {
                    state.PC += EA;
                }
            }
        }

        protected override Size getSize()
            {
                return getSizeFrom8BitImmediate();
            }
    }
}
