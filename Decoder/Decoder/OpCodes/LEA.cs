using System;

namespace Decoder.OpCodes
{
    /// <summary>
    /// 0100 rrr1 11mm mxxx
    /// </summary>
    class LEA : OpCode
    {
        public override string Name => "LEA";

        public override string Description => "Load Effective Address";

        public override string Operation => "<ea> -> An";

        public override string Syntax => string.Format("{0} <ea>, An", Name);

        public override string Assembly => string.Format("{0} {1}, {2}", Name, getEAString(decodeEA(), EA), getAn());

        public LEA(Data data, int address, ushort code)
            : base(data, address, code)
        {
            EA = readEA(decodeEA());
        }

        protected override AddressRegister getAn()
        {
            return (AddressRegister)code.GetBits(9, 3);
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
            return Size.Long;
        }

        protected override byte getXn()
        {
            return (byte)code.GetBits(0, 3);
        }
    }
}
