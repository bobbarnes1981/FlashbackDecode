using Decoder.OpCodes;
using System;

namespace Decoder
{
    class OpCodeDecoder
    {
        public OpCode Decode(MachineState state)
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


        private OpCode decode_0000(MachineState state)
        {
            switch (state.OpCode.GetBits(8, 4))
            {
                case 0x0006:
                    return new ADDI(state);

                // 0000 1100 xxxx xxxx
                case 0x000C:
                    return new CMPI(state);

                default:
                    throw new NotImplementedException();
            }
        }

        private OpCode decode_X000(MachineState state)
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

        private OpCode decode_4000(MachineState state)
        {
            switch(state.OpCode)
            {
                // 0100 1110 0111 0101
                case 0x4E75:
                    throw new NotImplementedException("RTS");
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

        private OpCode decode_4XX0(MachineState state)
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
                        return new EXT(state);
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
                        return new MOVEtoSR(state);
                    }

                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }

        private OpCode decode_5000(MachineState state)
        {
            switch (state.OpCode.GetBits(6, 2))
            {
                // 0101 xxxx 11xx xxxx
                case 0x3:
                    // Scc or DBcc
                    return decode_50X0(state);

                // 0101 xxxx ??xx xxxx
                case 0x0:
                case 0x1:
                case 0x2:
                    // ADDQ or SUBQ
                    switch (state.OpCode.GetBits(8, 1))
                    {
                        case 0x0:
                            return new ADDQ(state);

                        case 0x1:
                            //SUBQ
                            throw new NotImplementedException("SUBQ");

                        default:
                            throw new NotImplementedException();
                    }

                default:
                    throw new InvalidStateException();
            }
        }

        private OpCode decode_50X0(MachineState state)
        {
            switch (state.OpCode.GetBits(3, 3))
            {
                // xxxx xxxx xx00 1xxx
                case 0x1:
                    return new DBcc(state);

                // xxxx xxxx xx?? ?xxx
                default:
                    throw new NotImplementedException();
            }
        }

        private OpCode decode_6000(MachineState state)
        {
            switch (state.OpCode.GetBits(8, 4))
            {
                case 0x0000:
                    return new BRA(state);

                case 0x0001:
                    return new BSR(state);

                default:
                    return new Bcc(state);
            }
        }

        private OpCode decode_7000(MachineState state)
        {
            return new MOVEQ(state);
        }

        private OpCode decode_8000(MachineState state)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_9000(MachineState state)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_B000(MachineState state)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_C000(MachineState state)
        {
            switch (state.OpCode)
            {
                // 1100 0100 0000 0000
                case 0xC400:
                    return new AND(state);
            }

            switch (state.OpCode.GetBits(6, 3))
            {
                // 1100 xxx0 11xx xxxx
                case 0x3:
                    throw new NotImplementedException("MULU");

                default:
                    throw new NotImplementedException();
            }
        }

        private OpCode decode_D000(MachineState state)
        {
            // ADD, ADDX, ADDA

            switch (state.OpCode.GetBits(6, 2))
            {
                case 0x3:
                    return new ADDA(state);

                default:
                    throw new NotImplementedException();
            }
        }

        private OpCode decode_E000(MachineState state)
        {
            switch(state.OpCode.GetBits(6, 2))
            {
                case 0x3:
                    return decode_E0C0(state);

                default:
                    return decode_E0X0(state);
            }
        }

        private OpCode decode_E0C0(MachineState state)
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

        private OpCode decode_E0X0(MachineState state)
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
                    return new ROd(state);

                default:
                    throw new InvalidStateException();
            }
        }
    }
}
