using System;
using System.Text;

namespace Decoder
{
    class RomDecoder
    {
        bool running = true;

        private byte[] data;

        private int programCounter;

        private ushort opcodeRegister;

        public RomDecoder(byte[] data)
        {
            this.data = data;
        }

        /// <summary>
        /// Read 1 byte
        /// </summary>
        /// <param name="startAddress"></param>
        /// <returns></returns>
        private byte readByteFromData(int startAddress)
        {
            return data[startAddress];
        }

        /// <summary>
        /// Read 2 bytes
        /// </summary>
        /// <param name="startAddress"></param>
        /// <returns></returns>
        private ushort readWordFromData(int startAddress)
        {
            // big endian ordering of bytes in memory (https://en.wikipedia.org/wiki/Endianness)
            // 0x1234 stored as 12 34
            ushort word = 0x00;
            word |= (ushort)(data[startAddress + 0] << 8);
            word |= (ushort)(data[startAddress + 1] << 0);
            return word;
        }

        /// <summary>
        /// Read 4 bytes
        /// </summary>
        /// <param name="startAddress"></param>
        /// <returns></returns>
        private uint readLongFromData(int startAddress)
        {
            // big endian ordering of bytes in memory (https://en.wikipedia.org/wiki/Endianness)
            // 0x1234 stored as 12 34
            ushort word = 0x00;
            word |= (ushort)(data[startAddress + 0] << 24);
            word |= (ushort)(data[startAddress + 1] << 16);
            word |= (ushort)(data[startAddress + 2] << 8);
            word |= (ushort)(data[startAddress + 3] << 0);
            return word;
        }

        private string toBinary(ushort input)
        {
            StringBuilder builder = new StringBuilder();
            for (ushort u = 32768; u > 0; u = (ushort)(u / 2))
            {
                builder.AppendFormat("{0}", (input & u) == u ? 1 : 0);
            }

            return builder.ToString();
        }

        private void unhandledOpcode(ushort opcode)
        {
            throw new Exception("unhandle opcode");
        }

        enum OperationSize
        {
            // 8
            Byte,
            // 16
            Word,
            // 32
            Long
        }

        private OperationSize getOperationSizeA(ushort size)
        {
            switch (size)
            {
                case 0x0000:
                    return OperationSize.Byte;
                case 0x0001:
                    return OperationSize.Word;
                case 0x0002:
                    return OperationSize.Long;
                default:
                    throw new Exception(string.Format("invalid size {0:X4}", size));
            }
        }
        private OperationSize getOperationSizeB(ushort size)
        {
            switch (size)
            {
                case 0x0000:
                    return OperationSize.Word;
                case 0x0001:
                    return OperationSize.Long;
                default:
                    throw new Exception(string.Format("invalid size {0:X4}", size));
            }
        }
        private OperationSize getOperationSizeC(ushort size)
        {
            switch (size)
            {
                case 0x0001:
                    return OperationSize.Byte;
                case 0x0003:
                    return OperationSize.Word;
                case 0x0002:
                    return OperationSize.Long;
                default:
                    throw new Exception(string.Format("invalid size {0:X4}", size));
            }
        }

        enum AddressingMode
        {
            // 000
            Dn,
            // 001
            An,
            // 010
            _An_,
            // 011
            _An_p,
            // 100
            s_An_,
            // 101
            d16_An,
            // 110
            d8_An_Xn,

            // 111
            d16_PC,
            // 111
            d8_PC_Xn,
            // 111
            xxxW,
            // 111
            xxxL,
            // 111
            IMMEDIATE,
        }

        private AddressingMode getAddressingMode(ushort mode, ushort register)
        {
            switch ((AddressingMode)mode)
            {
                case AddressingMode.Dn:
                case AddressingMode.An:
                case AddressingMode._An_:
                case AddressingMode._An_p:
                case AddressingMode.s_An_:
                case AddressingMode.d16_An:
                case AddressingMode.d8_An_Xn:
                    return (AddressingMode)mode;

                default:
                    switch(register)
                    {
                        case 0x0000:
                            return AddressingMode.xxxW;
                        case 0x0001:
                            return AddressingMode.xxxL;
                        case 0x0002:
                            return AddressingMode.d16_PC;
                        case 0x0003:
                            return AddressingMode.d8_PC_Xn;
                        case 0x0004:
                            return AddressingMode.IMMEDIATE;
                    }
                    break;
            }

            throw new Exception();
        }

        enum Register
        {
            Address,
            Data
        }

        public void Decode(int origin)
        {
            programCounter = origin;

            do
            {
                Console.WriteLine("Addr\t0x{0:X4} ({0})", programCounter);

                opcodeRegister = readWordFromData(programCounter);
                programCounter += 2;

                decode();

            } while (running);
        }

        private void decode()
        {
            Console.WriteLine("OpCode\t0x{0:X4}", opcodeRegister);

            Console.WriteLine(toBinary(opcodeRegister));

            // check 0000 xxxx xxxx xxxx
            switch (opcodeRegister & 0xF000)
            {
                case 0x0000:
                    decode_0000();
                    break;

                case 0x1000:
                case 0x2000:
                case 0x3000:
                    decode_00XX();
                    break;

                case 0x4000:
                    decode_4000();
                    break;

                case 0x6000:
                    decode_6000();
                    break;

                default:
                    unhandledOpcode(opcodeRegister);
                    break;
            }

            Console.WriteLine();
        }

        private void decode_0000()
        {
            // check for xxxx 000x xxxx xxxx
            if ((opcodeRegister & 0x0E00) == 0x0000)
            {
                // check for xxxx xxx0 xxxx xxxx
                if ((opcodeRegister & 0x0100) == 0x0000)
                {
                    // check for xxxx xxxx xx11 1100
                    if ((opcodeRegister & 0x003F) == 0x003C)
                    {
                        unhandledOpcode(opcodeRegister);
                    }
                    else
                    {
                        opcode_ORI();
                    }
                }
                else
                {
                    unhandledOpcode(opcodeRegister);
                }
            }
            else
            {
                unhandledOpcode(opcodeRegister);
            }
        }

        private void decode_00XX()
        {
            // check for xxxx xxx0 01xx xxxx
            if ((opcodeRegister & 0x01C0) == 0x0040)
            {
                opcode_MOVEA();
            }
            else
            {
                opcode_MOVE();
            }
        }

        private void decode_4000()
        {
            // check for xxxx 100x xxxx xxxx
            if ((opcodeRegister & 0x0E00) == 0x0800)
            {
                // check for xxxx xxx? ??xx xxxx
                switch (opcodeRegister & 0x01C0)
                {
                    // xxxx xxx0 00xx xxxx
                    case 0x0000:
                        // NBCD
                        throw new NotImplementedException();
                        break;

                    // xxxx xxx0 01xx xxxx
                    case 0x0040:
                        // SWAP or PEA
                        throw new NotImplementedException();
                        break;

                    default:
                        // EXT or MOVEM
                        if ((ushort)(0x0007 & (opcodeRegister >> 3)) == 0x0000)
                        {
                            opcode_EXT();
                        }
                        else
                        {
                            opcode_MOVEM();
                        }
                        break;
                }
            }
        }

        private void decode_6000()
        {
            switch (opcodeRegister & 0x0F00)
            {
                // xxxx 0000 xxxx xxxx
                case 0x0000:
                    opcode_BRA();
                    break;

                // xxxx 0001 xxxx xxxx
                case 0x0001:
                    opcode_BSR();
                    break;

                default:
                    opcode_Bcc();
                    break;
            }
        }

        private void opcode_EXT()
        {
            Console.WriteLine("EXT");

            ushort mode = (ushort)((opcodeRegister >> 6) & 0x0007);

            if (mode != 0x0002 || mode != 0x0003)
            {
                throw new Exception(string.Format("invalid mode {0}", mode));
            } 

            ushort reg = (ushort)(opcodeRegister & 0x0007);

            Console.WriteLine("Register\tD{0}", reg);
        }

        private void opcode_BRA()
        {
            Console.WriteLine("BRA");

            ushort d = (ushort)(opcodeRegister & 0x00FF);

            if (d == 0x0000)
            {
                // 16 bit displacement
                short d16 = (short)readWordFromData(programCounter);
                programCounter += 2;
                programCounter += d16;

                Console.WriteLine("16Bit offset\t{0}", d16);
            }
            else
            {
                // 8 bit displacement
                sbyte d8 = (sbyte)d;
                programCounter += d8;

                Console.WriteLine("16Bit offset\t{0}", d8);

                throw new NotImplementedException();
            }
        }

        private void opcode_BSR()
        {
            Console.WriteLine("BSR");
            throw new NotImplementedException();
        }

        private void opcode_Bcc()
        {
            Console.WriteLine("Bcc");
            throw new NotImplementedException();
        }

        private void opcode_MOVE()
        {
            // MOVE
            Console.WriteLine("MOVE");

            // get bits 12-13 xxSS xxxx xxxx xxxx
            ushort size = (ushort)(0x0003 & (opcodeRegister >> 12));

            OperationSize s = getOperationSizeC(size);

            Console.WriteLine("Size\t{0}", s);
        }

        private void opcode_MOVEA()
        {
            // MOVEA
            Console.WriteLine("MOVEA");
        }

        private void opcode_MOVEM()
        {
            // MOVEM
            Console.WriteLine("MOVEM");

            ushort size = (ushort)((opcodeRegister >> 6) & 0x0001);

            OperationSize s = getOperationSizeB(size);

            switch(s)
            {
                case OperationSize.Word:
                    break;

                case OperationSize.Long:
                    break;

                default:
                    throw new Exception("invalid size {0}");
            }

            Console.WriteLine("Size\t{0}", s);

            ushort mask = readWordFromData(programCounter);
            programCounter += 2;
        }

        private void opcode_ORI()
        {
            // ORI
            Console.WriteLine("ORI");

            // get bits 6-7 xxxx xxxx SSxx xxxx
            ushort size = (ushort)(0x0003 & (opcodeRegister >> 6));

            OperationSize s = getOperationSizeA(size);

            Console.WriteLine("Size\t{0}", s);

            Console.Write("Data\t");
            switch (s)
            {
                case OperationSize.Byte:
                    // low order byte of immediate word
                    programCounter += 1;
                    Console.WriteLine("0x{0:X2}", readByteFromData(programCounter));
                    programCounter += 1;
                    break;
                case OperationSize.Word:
                    // whole immediate word
                    Console.WriteLine("0x{0:X4}", readWordFromData(programCounter));
                    programCounter += 2;
                    break;
                case OperationSize.Long:
                    // next two immediate words
                    Console.WriteLine("0x{0:X8}", readLongFromData(programCounter));
                    programCounter += 4;
                    break;
                default:
                    throw new Exception(string.Format("unhandled size {0}", s));
            }

            ushort mode = (ushort)(0x0007 & (opcodeRegister >> 3));

            ushort reg = (ushort)(0x0007 & (opcodeRegister >> 0));

            AddressingMode m = getAddressingMode(mode, reg);

            Console.WriteLine("Mode\t{0}", m);

            if (mode < 0x0007)
            {
                Register r;
                switch (m)
                {
                    case AddressingMode.Dn:
                        r = Register.Data;
                        break;

                    case AddressingMode._An_:
                    case AddressingMode._An_p:
                    case AddressingMode.s_An_:
                    case AddressingMode.d16_An:
                    case AddressingMode.d8_An_Xn:
                        r = Register.Address;
                        break;

                    default:
                        throw new Exception(string.Format("unhandled mode {0}", m));
                }

                Console.WriteLine("Reg\t{0}{1}", r, reg);
            }
        }
    }
}
