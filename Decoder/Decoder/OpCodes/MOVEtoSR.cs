namespace Decoder.OpCodes
{
    class MOVEtoSR : OpCode
    {
        protected override string definition => "0100011011mmmxxx";

        public override string Name => "MOVE";

        public override string Description => "Move to Status Register";

        public override string Operation => "<ea> -> SR";

        public override string Syntax => string.Format("{0} <ea>, SR", Name);

        public override string Assembly => string.Format("{0} {1}, SR", Name, getEAString(decodeEAMode(), EA));

        public MOVEtoSR(MachineState state)
            : base(state)
        {
            EA = readEA(decodeEAMode());
        }

        protected override Size getSize()
        {
            return Size.Word;
        }
    }
}
