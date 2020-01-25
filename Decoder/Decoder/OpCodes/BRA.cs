using System;

namespace Decoder.OpCodes
{
    /// <summary>
    /// 0110 0000 DDDD DDDD
    /// </summary>
    class BRA : OpCode
    {
        public BRA(Data data, int address, ushort code)
            : base(data, address, code, "BRA", "Branch Always PC + dn -> PC")
        {
            if (Size == Size.Word)
            {
                EA = readEA(EffectiveAddressMode.Immediate, 0x00);
                PCDisplacement -= 2; // remove auto increment
            }
            PCDisplacement += EA;
        }

        public override string Operation()
        {
            switch(Size)
            {
                case Size.Byte:
                    return string.Format("PC + {0} -> PC", (sbyte)EA);
                case Size.Word:
                    return string.Format("PC + {0} -> PC", (short)EA);
                default:
                    throw new Exception();
            }
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
            byte displacement = (byte)code.GetBits(8, 0);

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

        protected override byte getXn()
        {
            throw new NotImplementedException();
        }
    }
}
