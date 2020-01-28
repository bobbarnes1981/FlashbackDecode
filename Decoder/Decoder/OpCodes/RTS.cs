﻿namespace Decoder.OpCodes
{
    class RTS : OpCode
    {
        public override string Name => "RTS";

        public override string Description => "Return from Subroutine";

        public override string Operation => "(SP) -> PC; SP+4 -> SP";

        public override string Syntax => Name;

        public override string Assembly => Name;

        protected override string definition => "0100111001110101";

        public RTS(MachineState state)
            : base(state)
        {
            state.PC = 0x00;

            System.Console.WriteLine("{0:X8}", state.Read(state.SP + 0));
            System.Console.WriteLine("{0:X8}", state.Read(state.SP + 1));
            System.Console.WriteLine("{0:X8}", state.Read(state.SP + 2));
            System.Console.WriteLine("{0:X8}", state.Read(state.SP + 3));

            state.PC |= (byte)((state.Read(state.SP + 3) << 24) & 0xFF000000);
            state.PC |= (byte)((state.Read(state.SP + 2) << 16) & 0x00FF0000);
            state.PC |= (byte)((state.Read(state.SP + 1) << 8) & 0x0000FF00);
            state.PC |= (byte)((state.Read(state.SP + 0) << 0) & 0x000000FF);

            state.SP += 4;
        }

        protected override Size getSize()
        {
            return Size.None;
        }
    }
}
