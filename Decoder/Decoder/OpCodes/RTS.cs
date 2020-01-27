namespace Decoder.OpCodes
{
    class RTS : OpCode
    {
        public override string Name => "RTS";

        public override string Description => "Return from Subroutine";

        public override string Operation => "(SP) -> PC; SP+4 -> SP";

        public override string Syntax => Name;

        public override string Assembly => Name;

        protected override string definition => "0100111001110101";

        public RTS(MachineState state)
            : base(state)
        {
            state.PC = state.Read(state.SP);
            state.SP -= 4;
        }

        protected override Size getSize()
        {
            return Size.None;
        }
    }
}
