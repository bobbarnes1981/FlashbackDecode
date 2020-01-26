using System;

namespace Decoder.OpCodes
{
    class BRA : OpCode
    {
        protected override string definition => "01100000bbbbbbbb";

        public override string Name => "BRA";

        public override string Description => "Branch Always";

        public override string Operation => "PC + dn -> PC";

        public override string Syntax => string.Format("{0} <label>", Name);

        public override string Assembly
        {
            get
            {
                switch (Size)
                {
                    case Size.Byte:
                        return string.Format("{0} {1}", FullName, (sbyte)EA);
                    case Size.Word:
                        return string.Format("{0} {1}", FullName, (short)EA);
                    default:
                        throw new Exception();
                }
            }
        }

        public BRA(Data data, int address, ushort code)
            : base(data, address, code)
        {
            if (Size == Size.Word)
            {
                EA = readEA(EffectiveAddressMode.Immediate, 0x00);
                PCDisplacement -= 2; // remove auto increment
            }
            PCDisplacement += EA;
        }

        protected override Size getSize()
        {
            byte displacement = (byte)getImmediate();

            if (displacement == 0x00)
            {
                return Size.Word;
            }
            else
            {
                EA = (sbyte)displacement;
                return Size.Byte;
            }
        }
    }
}
