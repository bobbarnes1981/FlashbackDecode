namespace Decoder.OpCodes
{
    public class Bcc : OpCode
    {
        public override string Name => "B";

        public override string Description => "Branch Conditionally";

        public override string Operation => "If CONDITION TRUE PC+dn -> PC";

        public override string Syntax => $"{Name}{GetCondition()} <label>";

        public override string Assembly
        {
            get
            {
                switch (Size)
                {
                    case Size.Byte:
                        return $"{Name}{GetCondition()} {(sbyte)EffectiveAddress}";
                    case Size.Word:
                        return $"{Name}{GetCondition()} {(short)EffectiveAddress}";
                    default:
                        throw new InvalidStateException();
                }
            }
        }

        public Bcc(MachineState state)
            : base("0110ccccbbbbbbbb", state)
        {
            if (Size == Size.Word)
            {
                EffectiveAddress = readEA(EffectiveAddressMode.Immediate, 0x00);
            }

            if (CheckCondition())
            {
                if (Size == Size.Word)
                {
                    state.PC += EffectiveAddress - 2;
                }
                else
                {
                    state.PC += EffectiveAddress;
                }
            }
        }

        protected override Size getSize()
            {
                return getSizeFrom8BitImmediate();
            }
    }
}
