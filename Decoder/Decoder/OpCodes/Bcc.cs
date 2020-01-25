using System;

namespace Decoder.OpCodes
{
    class Bcc : OpCode
    {
        public Bcc(Data data, int address, ushort code)
            : base(data, address, code, "Bcc", "Branch Conditionally If CONDITION PC+dn -> PC")
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
