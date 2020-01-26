using System;

namespace Decoder.OpCodes
{
    class Invalid : OpCode
    {
        public override string Name => "INVALID";

        public override string Description => throw new NotImplementedException();

        public override string Operation => throw new NotImplementedException();

        public override string Syntax => throw new NotImplementedException();

        public override string Assembly => throw new NotImplementedException();

        public Invalid(Data data, int address, ushort code)
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
