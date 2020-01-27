namespace Decoder.OpCodes
{
    class ADDQ : OpCode
    {
        protected override string definition => "0101bbb0ssmmmxxx";

        public override string Name => "ADDQ";

        public override string Description => "Add Quick";

        public override string Operation => "Immediate Data + Destination -> Destination";

        public override string Syntax => string.Format("{0} #<data>, <ea>", Name);

        public override string Assembly => string.Format("{0} #{1}, {2}", FullName, getImmediate(), getEAString(decodeEAMode(), EA));

        public ADDQ(MachineState state)
            : base(state)
        {
            EA = readEA(decodeEAMode());

            // TODO: flags

            var dstVal = getEAValue(decodeEAMode(), EA);
            setEAValue(decodeEAMode(), getImmediate() + dstVal);
        }

        protected override Size getSize()
        {
            return (Size)getBits('s');
        }
    }
}
