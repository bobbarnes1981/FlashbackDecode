namespace Decoder.M68k
{
    using System;
    using Decoder.Exceptions;
    using Decoder.M68k.OpCodes;

    class OpCodeDecoder
    {
        public OpCode Decode(MegadriveState state)
        {
            switch (state.OpCode & 0xF000)
            {
                // 0000
                case 0x0000:
                    return decode_0000(state);

                // 0001
                // 0010
                // 0011
                case 0x1000:
                case 0x2000:
                case 0x3000:
                    return decode_X000(state);

                // 0100
                case 0x4000:
                    return decode_4000(state);

                // 0101
                case 0x5000:
                    return decode_5000(state);

                // 0110
                case 0x6000:
                    return decode_6000(state);

                // 0111
                case 0x7000:
                    return decode_7000(state);

                // 1000
                case 0x8000:
                    return decode_8000(state);

                // 1001
                case 0x9000:
                    return decode_9000(state);

                // 1011
                case 0xB000:
                    return decode_B000(state);

                // 1100
                case 0xC000:
                    return decode_C000(state);

                // 1101
                case 0xD000:
                    return decode_D000(state);

                // 1110
                case 0xE000:
                    return decode_E000(state);

                default:
                    return new Invalid(state);
            }
        }

        private OpCode decode_0000(MegadriveState state)
        {
            switch (state.OpCode.GetBits(8, 4))
            {
                // 0000 0110 xxxx xxxx
                case 0x0006:
                    throw new NotImplementedException("ADDI");

                // 0000 1100 xxxx xxxx
                case 0x000C:
                    throw new NotImplementedException("CMPI");

                // 0000 0010 xxxx xxxx
                case 0x0002:
                    return decode_0200(state);

                // TODO: BTST

                default:
                    throw new NotImplementedException();
            }
        }

        private OpCode decode_0200(MegadriveState state)
        {
            switch (state.OpCode.GetBits(0, 8))
            {
                // 0000 0010 0011 1100
                case 0x023C:
                    throw new NotImplementedException("ANDItoCCR");

                // 0000 0010 0111 1100
                case 0x027C:
                    throw new NotImplementedException("ANDItoSR");

                default:
                    return new ANDI(state);
            }
        }

        private OpCode decode_X000(MegadriveState state)
        {
            if (state.OpCode.GetBits(6, 3) == 0x0001)
            {
                return new MOVEA(state);
            }
            else
            {
                return new MOVE(state);
            }
        }

        private OpCode decode_4000(MegadriveState state)
        {
            switch(state.OpCode.GetBits(8, 4))
            {
                // 01001010xxxxxxxx
                case 0x000A:
                    return new TST(state);
            }

            // 0100 1110 0110 daaa  0x4E60
            ushort moveusp = 0x4E60;
            if ((state.OpCode & moveusp) == moveusp)
            {
                return new MOVEUSP(state);
            }

            // 0100 1110 0111 0101  0x4E75
            ushort rts = 0x4E75;

            if (state.OpCode == rts)
            {
                throw new NotImplementedException("RTS");
            }

            // 0100 ???? ???? ?xxx
            switch (state.OpCode.GetBits(6, 6))
            {
                // 100 001
                case 0x0021:
                    if (state.OpCode.GetBits(3, 3) == 0x0000)
                    {
                        // 100 001 000
                        throw new NotImplementedException("SWAP");
                    }

                    // 100 001 xxx
                    throw new NotImplementedException("PEA");
            }

            // xxxx xxx? ??xx xxxx
            switch (state.OpCode.GetBits(6, 3))
            {
                // xxxx xxx0 10xx xxxx
                // xxxx xxx0 11xx xxxx
                case 0x0002:
                case 0x0003:
                    return decode_4XX0(state);

                // xxxx xxx1 11xx xxxx
                case 0x0007:
                    return new LEA(state);

                default:
                    throw new NotImplementedException();
            }
        }

        private OpCode decode_4XX0(MegadriveState state)
        {
            switch (state.OpCode.GetBits(9, 3))
            {
                // xxxx 100x xxxx xxxx
                // xxxx 110x xxxx xxxx
                case 0x0004:
                case 0x0006:
                    // xxxx xxxx xx00 0xxx
                    if (state.OpCode.GetBits(3, 3) == 0x0000)
                    {
                        throw new NotImplementedException("EXT");
                    }
                    else
                    {
                        return new MOVEM(state);
                    }

                // xxxx 011x xxxx xxxx
                case 0x0003:
                    var b = state.OpCode.GetBits(6, 10);
                    if (state.OpCode.GetBits(6, 10) == 0x011B)
                    {
                        // 0100 0110 11xx xxxx
                        throw new NotImplementedException("MOVEtoSR");
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        private OpCode decode_5000(MegadriveState state)
        {
            // 0101 xxxx 11xx xxxx  0x50C0
            ushort sdb = 0x50C0;

            if ((state.OpCode & sdb) == sdb)
            {
                // Scc or DBcc
                return decode_50C0(state);
            }

            // 0101 xxxx ??xx xxxx

            // 0101 xxx0 xxxx xxxx
            ushort addq = 0x5000;

            if ((state.OpCode & addq) == addq)
            {
                throw new NotImplementedException("ADDQ");
            }

            // 0101 xxx1 xxxx xxxx
            ushort subq = 0x5100;

            if ((state.OpCode & subq) == subq)
            {
                throw new NotImplementedException("SUBQ");
            }

            throw new InvalidStateException();
        }

        private OpCode decode_50C0(MegadriveState state)
        {
            // 0101 xxxx 1100 1xxx
            ushort dbcc = 0x50C8;

            if ((state.OpCode & dbcc) == dbcc)
            {
                return new DBcc(state);
            }

            // 0101 xxxx 11?? ?xxx
            throw new NotImplementedException("Scc?");
        }

        private OpCode decode_6000(MegadriveState state)
        {
            // 0110 ---- xxxx xxxx
            switch (state.OpCode.GetBits(8, 4))
            {
                case 0x0000:
                    throw new NotImplementedException("BRA");

                case 0x0001:
                    throw new NotImplementedException("BSR");

                default:
                    return new Bcc(state);
            }
        }

        private OpCode decode_7000(MegadriveState state)
        {
            return new MOVEQ(state);
        }

        private OpCode decode_8000(MegadriveState state)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_9000(MegadriveState state)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_B000(MegadriveState state)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_C000(MegadriveState state)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_D000(MegadriveState state)
        {
            // adda
            // 1101 xxxx 11xx xxxx  0xD0C0
            ushort adda = 0xD0C0;

            if ((state.OpCode & adda) == adda)
            {
                throw new NotImplementedException("ADDA");
            }

            // 1101 xxx1 xx00 0xxx  0xD100 addx Dn
            ushort addxd = 0xD100;

            // 1101 xxx1 xx00 1xxx  0xD101 addx -(An)
            ushort addxa = 0xD108;

            if (((state.OpCode & addxd) == addxd) || ((state.OpCode & addxa) == addxa))
            {
                return new ADDX(state);
            }

            // add
            return new ADD(state);
        }

        private OpCode decode_E000(MegadriveState state)
        {
            switch(state.OpCode.GetBits(6, 2))
            {
                case 0x3:
                    return decode_E0C0(state);

                default:
                    return decode_E0X0(state);
            }
        }

        private OpCode decode_E0C0(MegadriveState state)
        {
            switch (state.OpCode.GetBits(9, 3))
            {
                case 0x0:
                    throw new NotImplementedException("ASd");

                case 0x1:
                    throw new NotImplementedException("LSd");

                case 0x2:
                    throw new NotImplementedException("ROXd");

                case 0x3:
                    throw new NotImplementedException("ROd");

                default:
                    throw new InvalidStateException();
            }
        }

        private OpCode decode_E0X0(MegadriveState state)
        {
            switch (state.OpCode.GetBits(3, 2))
            {
                case 0x0:
                    throw new NotImplementedException("ASd");

                case 0x1:
                    throw new NotImplementedException("LSd");

                case 0x2:
                    throw new NotImplementedException("ROXd");

                case 0x3:
                    throw new NotImplementedException("ROd");

                default:
                    throw new InvalidStateException();
            }
        }
    }
}
