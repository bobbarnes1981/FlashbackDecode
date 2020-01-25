using System;

namespace Decoder.OpCodes
{
    class MOVEtoSR : OpCode
    {
        public MOVEtoSR(Data data, int address, ushort code)
            : base(data, address, code, "MOVE", "Move to Status Register <ea> -> SR")
        {
            EA = readEA(decodeEA());
        }

        public override string Operation()
        {
            return string.Format("{0} -> SR", getEAString(decodeEA(), EA));
        }

        protected override AddressRegister getAn()
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
