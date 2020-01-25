using System;

namespace Decoder.OpCodes
{
    class EXT : OpCode
    {
        public EXT(Data data, int address, ushort code)
            : base(data, address, code, "EXT", "Sign Extend")
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
