using System;

namespace Decoder.OpCodes
{
    class Invalid : OpCode
    {
        public Invalid(Data data, int address, ushort code)
            : base(data, address, code, "INVALID", "INVALID OPCODE")
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
