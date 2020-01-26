using System;

namespace Decoder.OpCodes
{
    class MOVEA : OpCode
    {
        public override string Name => "MOVEA";

        public override string Description => "Move Address";

        public override string Operation => "<ea> -> An";

        public override string Syntax => throw new NotImplementedException();

        public override string Assembly => throw new NotImplementedException();

        public MOVEA(Data data, int address, ushort code)
            : base(data, address, code)
        {
        }

        protected override AddressRegister getAn()
        {
            throw new NotImplementedException();
        }

        protected override DataRegister getDn()
        {
            throw new NotImplementedException();
        }

        protected override byte getM()
        {
            throw new NotImplementedException();
        }

        protected override Size getSize()
        {
            throw new NotImplementedException();
        }

        protected override byte getXn()
        {
            throw new NotImplementedException();
        }
    }
}
