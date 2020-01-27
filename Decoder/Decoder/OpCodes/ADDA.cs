﻿namespace Decoder.OpCodes
{
    class ADDA : OpCode
    {
        protected override string definition => "1101aaas11mmmxxx";

        public override string Name => "ADDA";

        public override string Description => "Add Address";

        public override string Operation => "Source + Destination -> Destination";

        public override string Syntax => string.Format("{0} <ea>, An", Name);

        public override string Assembly => string.Format("{0} {1}, {2}", FullName, getEAString(decodeEA(), EA), getAn());

        public ADDA(MachineState state)
            : base(state)
        {
            EA = readEA(decodeEA());
        }

        protected override Size getSize()
        {
            return getSizeFromBits1(8);
        }
    }
}
