namespace Decoder.OpCodes
{
    /// <summary>
    /// 0100 rrr1 11mm mxxx
    /// </summary>
    class LEA : OpCode
    {
        public LEA(Data data, int address, ushort code)
            : base(data, address, code, "LEA", "Load Effective Address <ea> -> An")
        {
            EA = readEA(decodeEA());
        }

        public override string Operation()
        {
            return string.Format("{0} {1} -> {2}", FullName, getEAString(decodeEA(), EA), getAn());
        }

        protected override AddressRegister getAn()
        {
            return (AddressRegister)code.GetBits(9, 3);
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
