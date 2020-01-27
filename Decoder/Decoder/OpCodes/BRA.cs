﻿using System;

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
                        throw new InvalidStateException();
                }
            }
        }

        public BRA(MachineState state)
            : base(state)
        {
            if (Size == Size.Word)
            {
                EA = readEA(EffectiveAddressMode.Immediate, 0x00);
                state.PC -= 2; // remove auto increment
            }
            state.PC += EA;
        }

        protected override Size getSize()
        {
            return getSizeFrom8BitImmediate();
        }
    }
}
