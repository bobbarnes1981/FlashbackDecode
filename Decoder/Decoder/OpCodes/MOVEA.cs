using System;

namespace Decoder.OpCodes
{
    class MOVEA : OpCode
    {
        public MOVEA(Data data, int address, ushort code)
            : base(data, address, code, "MOVEA", "Move Address <ea> -> An")
        {
        }

        public override string Operation()
        {
            throw new NotImplementedException();
        }

        protected override AddressRegister getAn()
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
