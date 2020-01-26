﻿using System;

namespace Decoder.OpCodes
{
    class BSR : OpCode
    {
        protected override string definition => "01100001bbbbbbbb";

        public override string Name => "BSR";

        public override string Description => "Branch to Subroutine";

        public override string Operation => "SP-4 -> SP; PC -> (SP); PC+dn -> PC";

        public override string Syntax => throw new NotImplementedException();

        public override string Assembly => throw new NotImplementedException();

        public BSR(Data data, int address, ushort code)
            : base(data, address, code)
        {
        }

        protected override byte getM()
        {
            throw new NotImplementedException();
        }

        protected override Size getSize()
        {
            throw new NotImplementedException();
        }

        protected override byte getXn()
        {
            throw new NotImplementedException();
        }
    }
}
