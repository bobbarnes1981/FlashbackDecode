using System;

namespace Decoder.OpCodes
{
    class BSR : OpCode
    {
        public BSR(Data data, int address, ushort code)
            : base(data, address, code, "BSR", "Branch to Subroutine SP-4 -> SP; PC -> (SP); PC+dn -> PC")
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
