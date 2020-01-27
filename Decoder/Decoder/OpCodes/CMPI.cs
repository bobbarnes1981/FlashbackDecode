namespace Decoder.OpCodes
{
    class CMPI : OpCode
    {
        private uint immediate;

        public override string Name => "CMPI";

        public override string Description => "Compare Immediate";

        public override string Operation => "Destination - Immediate Data -> cc";

        public override string Syntax => string.Format("{0} #<data>, <ea>", Name);

        public override string Assembly => string.Format("{0} #{1}, {2}", Name, immediate, getEAString(decodeEA(), EA));

        protected override string definition => "00001100ssmmmxxx";

        public CMPI(MachineState state)
            : base(state)
        {
            immediate = readImmediate();
            EA = readEA(decodeEA());
        }

        protected override Size getSize()
        {
            return (Size)getBits('s');
        }
    }
}
