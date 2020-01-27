namespace Decoder.OpCodes
{
    class MOVEQ : OpCode
    {
        public override string Name => "MOVEQ";

        public override string Description => "Move Quick";

        public override string Operation => "Immediate data -> Destination";

        public override string Syntax => string.Format("{0} #<data>, Dn", Name);

        public override string Assembly => string.Format("{0} {1}, {2}", Name, getImmediate(), getDn());

        protected override string definition => "0111ddd0bbbbbbbb";

        public MOVEQ(MachineState state)
            : base(state)
        {
        }

        protected override Size getSize()
        {
            return Size.Long;
        }
    }
}
