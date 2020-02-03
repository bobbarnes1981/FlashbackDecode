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
            if (state.OpCode.GetBits(8, 1) == 0x1)
            {
                // MOVEP
                if (state.OpCode.GetBits(3, 3) == 0x1)
                {
                    throw new NotImplementedException("MOVEP");
                }

                // BTST, BCHG, BCLR, BSET
                switch (state.OpCode.GetBits(6, 2))
                {
                    case 0x00:
                        return new BTST(state);
                    case 0x01:
                        throw new NotImplementedException("BCHG");
                    case 0x02:
                        return new BCLR(state);
                    case 0x03:
                        throw new NotImplementedException("BSET");
                }
            }

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
            if (state.OpCode.CheckBits("01001010xxxxxxxx"))
            {
                return new TST(state);
            }

            if (state.OpCode.CheckBits("010011100110xxxx"))
            {
                return new MOVEUSP(state);
            }

            // 0x4E75
            if (state.OpCode.CheckBits("0100111001110101"))
            {
                throw new NotImplementedException("RTS");
            }

            if (state.OpCode.CheckBits("0100100001xxxxxx"))
            {
                if (state.OpCode.CheckBits("0100100001000xxx"))
                {
                    // 0100 100 001 000
                    throw new NotImplementedException("SWAP");
                }

                // 0100 100 001 xxx
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
            if (state.OpCode.CheckBits("0100100xxxxxxxxx") || state.OpCode.CheckBits("0100110xxxxxxxxx"))
            {
                if (state.OpCode.CheckBits("0100100xxx000xxx") || state.OpCode.CheckBits("0100110xxx000xxx"))
                {
                    throw new NotImplementedException("EXT");
                }
                else
                {
                    return new MOVEM(state);
                }
            }

            if (state.OpCode.CheckBits("0100011xxxxxxxxx"))
            {
                if (state.OpCode.CheckBits("0100011011xxxxxx"))
                {
                    return new MOVEtoSR(state);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            throw new NotImplementedException();
        }

        private OpCode decode_5000(MegadriveState state)
        {
            if (state.OpCode.CheckBits("0101xxxx11xxxxxx"))
            {
                // Scc or DBcc
                return decode_50C0(state);
            }

            // 0101 xxxx ??xx xxxx

            if (state.OpCode.CheckBits("0101xxx0xxxxxxxx"))
            {
                return new ADDQ(state);
            }

            if (state.OpCode.CheckBits("0101xxx1xxxxxxxx"))
            {
                throw new NotImplementedException("SUBQ");
            }

            throw new InvalidStateException();
        }

        private OpCode decode_50C0(MegadriveState state)
        {
            if (state.OpCode.CheckBits("0101xxxx11001xxx"))
            {
                return new DBcc(state);
            }

            // 0101 xxxx 11?? ?xxx
            throw new NotImplementedException("Scc?");
        }

        private OpCode decode_6000(MegadriveState state)
        {
            if (state.OpCode.CheckBits("01100000xxxxxxxx"))
            {
                return new BRA(state);
            }

            if (state.OpCode.CheckBits("01100001xxxxxxxx"))
            {
                throw new NotImplementedException("BSR");
            }

            return new Bcc(state);
        }

        private OpCode decode_7000(MegadriveState state)
        {
            return new MOVEQ(state);
        }

        private OpCode decode_8000(MegadriveState state)
        {
            // divu
            // 1000 ddd0 11mm mxxx
            if (state.OpCode.CheckBits("1000xxx011xxxxxx"))
            {
                throw new NotImplementedException("DIVU");
            }

            // divs
            // 1000 ddd1 11mm mxxx
            if (state.OpCode.CheckBits("1000xxx111xxxxxx"))
            {
                throw new NotImplementedException("DIVS");
            }

            // sbcd
            // 1000 xxx1 0000 Mxxx
            if (state.OpCode.CheckBits("1000xxx10000xxxx"))
            {
                throw new NotImplementedException("SBCD");
            }

            // or
            // 1000 dddD ssmm mxxx
            throw new NotImplementedException("OR");
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
            if (state.OpCode.CheckBits("1101xxxx11xxxxxx"))
            {
                throw new NotImplementedException("ADDA");
            }

            if (state.OpCode.CheckBits("1101xxx1xx000xxx") || state.OpCode.CheckBits("1101xxx1xx001xxx"))
            {
                return new ADDX(state);
            }

            // add
            return new ADD(state);
        }

        private OpCode decode_E000(MegadriveState state)
        {
            if (state.OpCode.CheckBits("1110xxxx11xxxxxx"))
            {
                return decode_E0C0(state);
            }

            return decode_E0X0(state);
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
