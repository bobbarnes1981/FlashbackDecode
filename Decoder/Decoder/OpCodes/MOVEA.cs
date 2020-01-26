using System;

namespace Decoder.OpCodes
{
    class MOVEA : OpCode
    {
        protected override string definition => "00ssaaa001mmmxxx";

        public override string Name => "MOVEA";

        public override string Description => "Move Address";

        public override string Operation => "<ea> -> An";

        public override string Syntax => throw new NotImplementedException();

        public override string Assembly => throw new NotImplementedException();

        public MOVEA(Data data, int address, ushort code)
            : base(data, address, code)
        {
            EA = readEA(decodeEA());
        }

        protected override Size getSize()
        {
            throw new NotImplementedException();
        }
    }
}
