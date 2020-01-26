﻿using System;

namespace Decoder.OpCodes
{
    class MOVEM : OpCode
    {
        protected override string definition => "01001D001smmmxxx";

        private ushort mask;

        public override string Name => "MOVEM";

        public override string Description => "Move Multiple Registers";

        public override string Operation => "<list> -> <ea> or <ea> -> <list>";

        public override string Syntax => string.Format("{0} <list> , <ea>\r\n{0} <ea>, <list>", Name);

        public override string Assembly
        {
            get
            {
                switch (getDirection())
                {
                    case CopyDirection.MemoryToRegister:
                        return string.Format("{0} {1}, {2}", Name, getEAString(decodeEA(), EA), mask.ToBinary());

                    case CopyDirection.RegisterToMemory:
                        return string.Format("{0} {2}, {1}", Name, getEAString(decodeEA(), EA), mask.ToBinary());

                    default:
                        throw new Exception();
                }
            }
        }

        public MOVEM(Data data, int address, ushort code)
            : base(data, address, code)
        {
            mask = data.ReadWord(address + PCDisplacement);
            PCDisplacement += 2;

            EA = readEA(decodeEA());
        }

        protected override Size getSize()
        {
            return getSizeFromBits1(6);
        }

        protected CopyDirection getDirection()
        {
            return (CopyDirection)getBits('D');
        }
    }
}
