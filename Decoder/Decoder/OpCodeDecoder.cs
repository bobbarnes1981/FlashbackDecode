using Decoder.OpCodes;
using System;

namespace Decoder
{
    class OpCodeDecoder
    {
        public OpCode Decode(Data data, int address, ushort opcode)
        {
            switch (opcode & 0xF000)
            {
                // 0000
                case 0x0000:
                    return decode_0000(data, address, opcode);

                // 0001
                // 0010
                // 0011
                case 0x1000:
                case 0x2000:
                case 0x3000:
                    return decode_X000(data, address, opcode);

                // 0100
                case 0x4000:
                    return decode_4000(data, address, opcode);

                // 0101
                case 0x5000:
                    return decode_5000(data, address, opcode);

                // 0110
                case 0x6000:
                    return decode_6000(data, address, opcode);

                // 0111
                case 0x7000:
                    return decode_7000(data, address, opcode);

                // 1000
                case 0x8000:
                    return decode_8000(data, address, opcode);

                // 1001
                case 0x9000:
                    return decode_9000(data, address, opcode);

                // 1011
                case 0xB000:
                    return decode_B000(data, address, opcode);

                // 1100
                case 0xC000:
                    return decode_C000(data, address, opcode);

                // 1101
                case 0xD000:
                    return decode_D000(data, address, opcode);

                // 1110
                case 0xE000:
                    return decode_E000(data, address, opcode);

                default:
                    return new Invalid(data, address, opcode);
            }
        }


        private OpCode decode_0000(Data data, int address, ushort opcode)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_X000(Data data, int address, ushort opcode)
        {
            if (opcode.GetBits(6, 3) == 0x0001)
            {
                return new MOVEA(data, address, opcode);
            }
            else
            {
                return new MOVE(data, address, opcode);
            }
        }

        private OpCode decode_4000(Data data, int address, ushort opcode)
        {
            // xxxx xxx? ??xx xxxx
            switch (opcode.GetBits(6, 3))
            {
                // xxxx xxx0 10xx xxxx
                // xxxx xxx0 11xx xxxx
                case 0x0002:
                case 0x0003:
                    return decode_4XX0(data, address, opcode);

                // xxxx xxx1 11xx xxxx
                case 0x0007:
                    return new LEA(data, address, opcode);

                default:
                    throw new NotImplementedException();
            }
        }

        private OpCode decode_4XX0(Data data, int address, ushort opcode)
        {
            switch (opcode.GetBits(9, 3))
            {
                // xxxx 100x xxxx xxxx
                // xxxx 110x xxxx xxxx
                case 0x0004:
                case 0x0006:
                    // xxxx xxxx xx00 0xxx
                    if (opcode.GetBits(3, 3) == 0x0000)
                    {
                        return new EXT(data, address, opcode);
                    }
                    else
                    {
                        return new MOVEM(data, address, opcode);
                    }

                // xxxx 011x xxxx xxxx
                case 0x0003:
                    var b = opcode.GetBits(6, 10);
                    if (opcode.GetBits(6, 10) == 0x011B)
                    {
                        // 0100 0110 11xx xxxx
                        return new MOVEtoSR(data, address, opcode);
                    }

                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }

        private OpCode decode_5000(Data data, int address, ushort opcode)
        {
            switch (opcode.GetBits(6, 2))
            {
                // 0101 xxxx 11xx xxxx
                case 0x3:
                    // Scc or DBcc
                    return decode_50X0(data, address, opcode);

                // 0101 xxxx ??xx xxxx
                case 0x0:
                case 0x1:
                case 0x2:
                    // ADDQ or SUBQ
                    throw new NotImplementedException();
                    break;

                default:
                    throw new Exception();
            }
        }

        private OpCode decode_50X0(Data data, int address, ushort opcode)
        {
            switch (opcode.GetBits(3, 3))
            {
                // xxxx xxxx xx00 1xxx
                case 0x1:
                    return new DBcc(data, address, opcode);

                // xxxx xxxx xx?? ?xxx
                default:
                    throw new NotImplementedException();
                    break;

            }
        }

        private OpCode decode_6000(Data data, int address, ushort opcode)
        {
            switch (opcode.GetBits(8, 4))
            {
                case 0x0000:
                    return new BRA(data, address, opcode);

                case 0x0001:
                    return new BSR(data, address, opcode);

                default:
                    return new Bcc(data, address, opcode);
            }
        }

        private OpCode decode_7000(Data data, int address, ushort opcode)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_8000(Data data, int address, ushort opcode)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_9000(Data data, int address, ushort opcode)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_B000(Data data, int address, ushort opcode)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_C000(Data data, int address, ushort opcode)
        {
            throw new NotImplementedException();
        }

        private OpCode decode_D000(Data data, int address, ushort opcode)
        {
            // ADD, ADDX, ADDA

            switch (opcode.GetBits(6, 2))
            {
                case 0x3:
                    return new ADDA(data, address, opcode);

                default:
                    throw new NotImplementedException();
            }
        }

        private OpCode decode_E000(Data data, int address, ushort opcode)
        {
            throw new NotImplementedException();
        }
    }
}
