using System;

namespace Decoder.OpCodes
{
    /// <summary>
    /// 00SS XXX MMM MMM XXX
    /// </summary>
    public class MOVE : OpCode
    {
        protected override string definition => "00ss______mmmxxx";

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
                    getEAString(decodeEAMode(getSrcM(), getSrcXn()), SrcEA, getSrcXn()),
                    getEAString(decodeEAMode(getDstM(), getDstXn()), DstEA, getDstXn())
                );
            }
        }

        public int SrcEA { get; protected set; }
        public int DstEA { get; protected set; }

        public MOVE(MachineState state)
            : base(state)
        {
            SrcEA = readEA(decodeEAMode(getSrcM(), getSrcXn()), getSrcXn());
            DstEA = readEA(decodeEAMode(getDstM(), getDstXn()), getDstXn());

            var srcVal = getEAValue(decodeEAMode(getSrcM(), getSrcXn()), SrcEA);
            setEAValue(decodeEAMode(getDstM(), getDstXn()), DstEA, srcVal);
        }

        protected byte getSrcM()
        {
            return getM();
        }

        protected byte getDstM()
        {
            return (byte)state.OpCode.GetBits(6, 3);
        }

        protected override Size getSize()
        {
            switch (state.OpCode.GetBits(12, 2))
            {
                case 0x0001:
                    return Size.Byte;

                case 0x0002:
                    return Size.Long;

                case 0x0003:
                    return Size.Word;

                default:
                    throw new InvalidStateException();
            }
        }

        protected byte getSrcXn()
        {
            return getXn();
        }

        protected byte getDstXn()
        {
            return (byte)state.OpCode.GetBits(9, 3);
        }
    }
}
