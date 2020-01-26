﻿namespace Decoder.OpCodes
{
    class ADDI : OpCode
    {
        private uint immediate;

        protected override string definition => "00000110ssmmmxxx";

        public override string Name => "ADDI";

        public override string Description => "Add Immediate";

        public override string Operation => "Immediate Data + Destination -> Destination";

        public override string Syntax => string.Format("{0} #<data>, <ea>", Name);

        public override string Assembly => string.Format("{0} #{1}, {2}", FullName, immediate, getEAString(decodeEA(), EA));

        public ADDI(Data data, int address, ushort code)
            : base(data, address, code)
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
