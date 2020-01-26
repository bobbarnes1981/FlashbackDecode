using System;

namespace Decoder.OpCodes
{
    class Bcc : OpCode
    {
        public override string Name => "Bcc";

        public override string Description => "Branch Conditionally";

        public override string Operation => "If CONDITION TRUE PC+dn -> PC";

        public override string Syntax => throw new NotImplementedException();

        public override string Assembly => throw new NotImplementedException();

        public Bcc(Data data, int address, ushort code)
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
