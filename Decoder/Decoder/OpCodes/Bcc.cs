using System;

namespace Decoder.OpCodes
{
    class Bcc : OpCode
    {
        protected override string definition => "0110ccccbbbbbbbb";

        public override string Name => "Bcc";

        public override string Description => "Branch Conditionally";

        public override string Operation => "If CONDITION TRUE PC+dn -> PC";

        public override string Syntax => string.Format("{0} <label>", Name);

        public override string Assembly
        {
            get
            {
                switch (Size)
                {
                    case Size.Byte:
                        return string.Format("{0} {1}", Name, (sbyte)EA);
                    case Size.Word:
                        return string.Format("{0} {1}", Name, (short)EA);
                    default:
                        throw new InvalidStateException();
                }
            }
        }

        public Bcc(Data data, int address, ushort code)
            : base(data, address, code)
        {
            if (Size == Size.Word)
            {
                EA = readEA(EffectiveAddressMode.Immediate, 0x00);
                //PCDisplacement -= 2; // remove auto increment
            }
            //PCDisplacement += EA;
        }

        protected override Size getSize()
        {
            return getSizeFrom8BitImmediate();
        }
    }
}
