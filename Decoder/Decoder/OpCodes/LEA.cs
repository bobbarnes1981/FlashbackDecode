namespace Decoder.OpCodes
{
    class LEA : OpCode
    {
        protected override string definition => "0100aaa111mmmxxx";

        public override string Name => "LEA";

        public override string Description => "Load Effective Address";

        public override string Operation => "<ea> -> An";

        public override string Syntax => string.Format("{0} <ea>, An", Name);

        public override string Assembly => string.Format("{0} {1}, {2}", Name, getEAString(decodeEA(), EA), getAn());

        public LEA(MachineState state)
            : base(state)
        {
            EA = readEA(decodeEA());
        }

        protected override Size getSize()
        {
            return Size.Long;
        }
    }
}
