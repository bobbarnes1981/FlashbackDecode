using System;

namespace Decoder.OpCodes
{
    /// <summary>
    /// 00SS XXX MMM MMM XXX
    /// </summary>
    class MOVE : OpCode
    {
        public override string Name => "MOVE";

        public override string Description => "Move Data from Source to Destination";

        public override string Operation => "<ea> -> <ea>";

        public override string Syntax => string.Format("{0} <ea>, <ea>", Name);

        public override string Assembly
        {
            get
            {
                return string.Format("{0} {1}, {2}",
                    FullName,
                    getEAString(decodeEA(getSrcM(), getSrcXn()), SrcAddress, getSrcXn()),
                    getEAString(decodeEA(getDstM(), getDstXn()), DstAddress, getDstXn())
                );
            }
        }

        public int SrcAddress { get; protected set; }
        public int DstAddress { get; protected set; }

        public MOVE(Data data, int address, ushort code)
            : base(data, address, code)
        {
            SrcAddress = readEA(decodeEA(getSrcM(), getSrcXn()), getSrcXn());
            DstAddress = readEA(decodeEA(getDstM(), getDstXn()), getDstXn());
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

        protected byte getSrcM()
        {
            return (byte)code.GetBits(3, 3);
        }

        protected byte getDstM()
        {
            return (byte)code.GetBits(6, 3);
        }

        protected override Size getSize()
        {
            switch (code.GetBits(12, 2))
            {
                case 0x0001:
                    return Size.Byte;

                case 0x0002:
                    return Size.Long;

                case 0x0003:
                    return Size.Word;

                default:
                    throw new Exception();
            }
        }

        protected override byte getXn()
        {
            throw new NotImplementedException();
        }

        protected byte getSrcXn()
        {
            return (byte)code.GetBits(0, 3);
        }

        protected byte getDstXn()
        {
            return (byte)code.GetBits(9, 3);
        }
    }
}
