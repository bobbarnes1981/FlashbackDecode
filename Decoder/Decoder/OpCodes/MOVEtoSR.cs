using System;

namespace Decoder.OpCodes
{
    class MOVEtoSR : OpCode
    {
        public override string Name => "MOVE";

        public override string Description => "Move to Status Register";

        public override string Operation => "<ea> -> SR";

        public override string Syntax => string.Format("{0} <ea>, SR", Name);

        public override string Assembly => string.Format("{0} {1}, SR", Name, getEAString(decodeEA(), EA));

        public MOVEtoSR(Data data, int address, ushort code)
            : base(data, address, code)
        {
            EA = readEA(decodeEA());
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
            return (byte)code.GetBits(3, 3);
        }

        protected override Size getSize()
        {
            return Size.Word;
        }

        protected override byte getXn()
        {
            return (byte)code.GetBits(0, 3);
        }
    }
}
