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
        /// <returns></returns>
        private byte readByteFromData()
        {
            byte b = data[programCounter + 0];

            programCounter += 1;

            return b;
        }

        /// <summary>
        /// Read 2 bytes
        /// </summary>
        /// <returns></returns>
        private ushort readWordFromData()
        {
            // big endian ordering of bytes in memory (https://en.wikipedia.org/wiki/Endianness)
            // 0x1234 stored as 12 34
            ushort word = 0x00;
            word |= (ushort)(data[programCounter + 0] << 8);
            word |= (ushort)(data[programCounter + 1] << 0);

            programCounter += 2;

            return word;
        }

        /// <summary>
        /// Read 4 bytes
        /// </summary>
        /// <returns></returns>
        private uint readLongFromData()
        {
            // big endian ordering of bytes in memory (https://en.wikipedia.org/wiki/Endianness)
            // 0x1234 stored as 12 34
            ushort word = 0x00;
            word |= (ushort)(data[programCounter + 0] << 24);
            word |= (ushort)(data[programCounter + 1] << 16);
            word |= (ushort)(data[programCounter + 2] << 8);
            word |= (ushort)(data[programCounter + 3] << 0);

            programCounter += 4;

            return word;
        }

        private uint readDataFromData(Size size)
        {
            switch(size)
            {
                case Size.Byte:
                    return readByteFromData();

                case Size.Word:
                    return readWordFromData();

                case Size.Long:
                    return readLongFromData();

                default:
                    throw new Exception();
            }
        }

        private uint readAddressFromData(AddressingMode mode, Size size)
        {
            if ((int)mode < 7)
            {
                throw new Exception();
            }

            switch (mode)
            {
                case AddressingMode.AbsoluteWord:
                    return readWordFromData();

                case AddressingMode.Immediate:
                    return readDataFromData(size);

                default:
                    throw new NotImplementedException();
            }
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

        public void Decode(int origin)
        {
            programCounter = origin;

            do
            {
                Console.WriteLine("Addr\t0x{0:X4} ({0})", programCounter);

                opcodeRegister = readWordFromData();

                decode();

            } while (running);
        }

        private void decode()
        {
            Console.WriteLine("OpCode\t0x{0:X4}", opcodeRegister);

            Console.WriteLine(toBinary(opcodeRegister));

            switch (opcodeRegister)
            {
                case 0x2700:
                case 0x31FC:
                    opcode_MOVE();
                    break;

                case 0x48F8:
                    opcode_MOVEM();
                    break;

                case 0x6000:
                    opcode_BRA();
                    break;

                default:
                    unhandledOpcode(opcodeRegister);
                    break;
            }

            Console.WriteLine();
        }

        enum AddressingMode
        {
            DataRegister,
            AddressRegister,
            Address,
            Address_PostIncremenet,
            Address_PreDecrement,
            AddressWithDisplacement,
            AddressWithIndex,

            AbsoluteWord = 0x10,                // 000
            AbsoluteLong = 0x11,                // 001

            ProgramCounter_Displacement = 0x12, // 010
            ProgramCounter_Index = 0x13,        // 011

            Immediate = 0x14,                   // 100
        }

        enum Size
        {
            Byte,
            Word,
            Long
        }

        private AddressingMode effectiveAddress(ushort m, ushort Xn)
        {
            if (m < 7)
            {
                return (AddressingMode)m;
            }
            else if (m == 7)
            {
                if (Xn > 4)
                {
                    throw new Exception(string.Format("invalid Xn {0}", Xn));
                }

                return (AddressingMode)(0x10 | Xn);
            }
            else
            {
                throw new Exception(string.Format("invalid m {0}", m));
            }
        }

        private void opcode_BRA()
        {
            Console.Write("BRA");

            // 0110 0000 DDDD DDDD

            byte displacement = (byte)(opcodeRegister & 0x00FF);

            Size size;
            if (displacement == 0x00)
            {
                Console.WriteLine(".w\t(WORD)");
                size = Size.Word;

                short offsetW = (short)readWordFromData();
                Console.WriteLine("0x{0:X4} ({0})", offsetW);
                programCounter += offsetW;
            }
            else
            {
                Console.WriteLine(".b\t(BYTE)");
                size = Size.Byte;

                sbyte offsetB = (sbyte)readByteFromData();
                Console.WriteLine("0x{0:X2} ({0})", offsetB);
            }
        }

        private void opcode_MOVE()
        {
            Console.Write("MOVE");

            // 00SS XXX MMM MMM XXX

            Size size;
            switch ((opcodeRegister >> 12) & 0x0003)
            {
                case 0x0000:
                    throw new Exception();
                case 0x0001:
                    Console.WriteLine(".b\t(BYTE)");
                    size = Size.Byte;
                    break;
                case 0x0002:
                    Console.WriteLine(".l\t(LONG)");
                    size = Size.Long;
                    break;
                case 0x0003:
                    Console.WriteLine(".w\t(WORD)");
                    size = Size.Word;
                    break;
                default:
                    throw new Exception();
            }

            // decode effective address

            ushort srcM = (ushort)((opcodeRegister >> 6) & 0x0007);
            ushort srcXn = (ushort)((opcodeRegister >> 9) & 0x0007);
            var src = effectiveAddress(srcM, srcXn);

            Console.WriteLine("src");
            Console.WriteLine("EAddr\t{0}", src);
            if ((int)src < 7)
            {
                Console.WriteLine("Reg\t{0}", srcXn);
            }
            else
            {
                uint s = readAddressFromData(src, size);
                Console.WriteLine("0x{0:X4}", s);
            }

            ushort dstM = (ushort)((opcodeRegister >> 3) & 0x0007);
            ushort dstXn = (ushort)((opcodeRegister >> 0) & 0x0007);
            var dst = effectiveAddress(dstM, dstXn);

            Console.WriteLine("dst");
            Console.WriteLine("EAddr\t{0}", dst);
            if ((int)dst < 7)
            {
                Console.WriteLine("Reg\t{0}", dstXn);
            }
            else
            {
                uint d = readAddressFromData(dst, size);
                Console.WriteLine("0x{0:X4}", d);
            }

            // TODO: do the move
        }

        private void opcode_MOVEM()
        {
            Console.Write("MOVE");

            // 00SS XXX MMM MMM XXX

            Size size;
            switch ((opcodeRegister >> 6) & 0x0001)
            {
                case 0x0000:
                    Console.WriteLine(".w\t(WORD)");
                    size = Size.Word;
                    break;
                case 0x0001:
                    Console.WriteLine(".l\t(LONG)");
                    size = Size.Long;
                    break;
                default:
                    throw new Exception();
            }

            // decode effective address

            AddressingMode src;
            switch ((opcodeRegister >> 10) & 0x0001)
            {
                case 0x0000:
                    src = AddressingMode.DataRegister;
                    break;
                case 0x0001:
                    src = AddressingMode.Address_PreDecrement;
                    break;
                default:
                    throw new Exception();
            }

            ushort mask = readWordFromData();
            Console.WriteLine("Mask\t{0:X4}", mask);

            Console.WriteLine("src");
            Console.WriteLine("EAddr\t{0}", src);
            //if ((int)src < 7)
            //{
            //    Console.WriteLine("Reg\t{0}", srcXn);
            //}
            //else
            //{
            //    uint s = readAddressFromData(src, size);
            //    Console.WriteLine("0x{0:X4}", s);
            //}

            ushort dstM = (ushort)((opcodeRegister >> 3) & 0x0007);
            ushort dstXn = (ushort)((opcodeRegister >> 0) & 0x0007);
            var dst = effectiveAddress(dstM, dstXn);

            Console.WriteLine("dst");
            Console.WriteLine("EAddr\t{0}", dst);
            if ((int)dst < 7)
            {
                Console.WriteLine("Reg\t{0}", dstXn);
            }
            else
            {
                uint d = readAddressFromData(dst, size);
                Console.WriteLine("0x{0:X4}", d);
            }

            throw new NotImplementedException();
            // TODO: do the move
        }
    }
}
