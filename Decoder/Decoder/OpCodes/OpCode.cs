using System;

namespace Decoder.OpCodes
{
    abstract class OpCode
    {
        protected abstract string definition { get; }

        protected readonly MachineState state;

        public readonly int Address;

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string Operation { get; }

        public abstract string Syntax { get; }

        public abstract string Assembly { get; }

        public readonly Size Size;

        // TODO: int?
        public int EA { get; protected set; }

        // TODO: add operation length so we can skip addresses in disassembly

        public string FullName
        {
            get
            {
                return string.Format("{0}.{1}", Name, Size.ToString().ToLower()[0]);
            }
        }

        public OpCode(MachineState state)
        {
            this.state = state;

            Address = state.PC;

            Size = getSize();
        }

        protected abstract Size getSize();

        protected Size getSizeFromBits1(int offset)
        {
            switch (state.OpCode.GetBits(offset, 1))
            {
                case 0x0000:
                    return Size.Word;
                case 0x0001:
                    return Size.Long;
                default:
                    throw new InvalidStateException();
            }
        }

        protected Size getSizeFrom8BitImmediate()
        {
            byte displacement = (byte)getImmediate();

            if (displacement == 0x00)
            {
                return Size.Word;
            }
            else
            {
                EA = (sbyte)displacement;
                return Size.Byte;
            }
        }

        private Tuple<int, int> getValues(char c)
        {
            int offset = -1;
            int length = -1;
            for (int i = 0; i < 16; i++)
            {
                if (definition[15-i] == c)
                {
                    if (offset == -1)
                    {
                        offset = i;
                        length = 0;
                    }
                    length++;
                }
            }

            if (offset == -1 || length == -1)
            {
                throw new Exception(string.Format("{0} not defined", c));
            }

            return new Tuple<int, int>(offset, length);
        }

        protected ushort getBits(char c)
        {
            var vals = getValues(c);
            return state.OpCode.GetBits(vals.Item1, vals.Item2);
        }

        protected AddressRegister getAn()
        {
            return (AddressRegister)getBits('a'); // Address register 'An'
        }

        protected DataRegister getDn()
        {
            return (DataRegister)getBits('d'); // Data register 'Dn'
        }

        protected ushort getImmediate()
        {
            return getBits('b'); // Immediate data (in opcode)
        }

        protected uint readImmediate()
        {
            return (uint)readData(Size);
        }

        protected byte getM()
        {
            return (byte)getBits('m'); // Mode 'M'
        }

        protected byte getXn()
        {
            return (byte)getBits('x'); // Reg number 'Xn' 
        }

        protected EffectiveAddressMode decodeEA()
        {
            return decodeEA(getM(), getXn());
        }

        protected EffectiveAddressMode decodeEA(byte M, byte Xn)
        {
            // TODO: validate allowed addressing modes

            if (M < 0x07)
            {
                return (EffectiveAddressMode)(M << 3);
            }
            else
            {
                return (EffectiveAddressMode)(M << 3 | Xn);
            }
        }

        protected string getEAString(EffectiveAddressMode mode, int ea)
        {
            return getEAString(mode, ea, getXn());
        }

        protected string getEAString(EffectiveAddressMode mode, int ea, byte xn)
        {
            switch (mode)
            {
                case EffectiveAddressMode.Immediate:
                    return string.Format("#{0}", ea);

                case EffectiveAddressMode.AbsoluteWord:
                    return string.Format("0x{0:X4}", ea);

                case EffectiveAddressMode.AbsoluteLong:
                    return string.Format("0x{0:X8}", ea);

                case EffectiveAddressMode.AddressWithDisplacement:
                    return string.Format("(d16, A{0})", xn);

                case EffectiveAddressMode.Address:
                    return string.Format("(A{0})", xn);

                case EffectiveAddressMode.Address_PostIncremenet:
                    return string.Format("(A{0}+)", xn);

                case EffectiveAddressMode.DataRegister:
                    return string.Format("D{0}", xn);

                case EffectiveAddressMode.AddressRegister:
                    return string.Format("A{0}", xn);

                default:
                    throw new System.NotImplementedException(mode.ToString());
            }
        }

        protected int readEA(EffectiveAddressMode ea)
        {
            return readEA(ea, getXn());
        }

        protected int readEA(EffectiveAddressMode ea, byte Xn)
        {
            switch (ea)
            {
                case EffectiveAddressMode.Immediate:
                    return readData(Size);

                case EffectiveAddressMode.AbsoluteWord:
                    return readData(Size.Word);

                case EffectiveAddressMode.AbsoluteLong:
                    return readData(Size.Long);

                case EffectiveAddressMode.DataRegister:
                    return Xn;

                case EffectiveAddressMode.AddressWithDisplacement:
                    short displacement = (short)readData(Size.Word);
                    return readAReg(Xn) + displacement;

                case EffectiveAddressMode.Address:
                    return readAReg(Xn);

                case EffectiveAddressMode.AddressRegister:
                    return Xn;

                case EffectiveAddressMode.Address_PostIncremenet:
                    ushort api = readAReg(Xn);
                    writeAReg(Xn, (ushort)(api + 1));
                    return api;

                default:
                    throw new NotImplementedException(ea.ToString());
            }
        }

        private ushort readAReg(byte register)
        {
            Console.WriteLine("WARNING: register An {0} reading not implemented", register);
            return 0x00;
        }

        private void writeAReg(byte register, ushort data)
        {
            Console.WriteLine("WARNING: register An {0} writing not implemented", register);
        }

        protected int readData(Size size)
        {
            switch (size)
            {
                case Size.Long:
                    uint l = state.ReadLong(state.PC);
                    state.PC += 4;
                    return (int)l;

                case Size.Word:
                    ushort w = state.ReadWord(state.PC);
                    state.PC += 2;
                    return (int)w;

                case Size.Byte:
                    // discard first byte
                    state.ReadByte(state.PC);
                    state.PC += 1;
                    byte b = (byte)state.ReadByte(state.PC);
                    state.PC += 1;
                    return (int)b;

                default:
                    throw new InvalidStateException();
            }
        }
    }
}
