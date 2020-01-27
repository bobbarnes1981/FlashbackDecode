namespace Decoder.OpCodes
{
    class ADDI : OpCode
    {
        private uint immediate;

        protected override string definition => "00000110ssmmmxxx";

        public override string Name => "ADDI";

        public override string Description => "Add Immediate";

        public override string Operation => "Immediate Data + Destination -> Destination";

        public override string Syntax => string.Format("{0} #<data>, <ea>", Name);

        public override string Assembly => string.Format("{0} #{1}, {2}", FullName, immediate, getEAString(decodeEAMode(), EA));

        public ADDI(MachineState state)
            : base(state)
        {
            immediate = readImmediate();
            EA = readEA(decodeEAMode());

            // TODO: flags

            var dstVal = getEAValue(decodeEAMode(), EA);
            setEAValue(decodeEAMode(), immediate + dstVal);
        }

        protected override Size getSize()
        {
            return (Size)getBits('s');
        }
    }
}
