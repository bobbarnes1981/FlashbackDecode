using System;

namespace Decoder.OpCodes
{
    public abstract class OpCode
    {
        protected abstract string definition { get; }

        protected readonly MachineState state;

        public readonly uint Address;

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string Operation { get; }

        public abstract string Syntax { get; }

        public abstract string Assembly { get; }

        public readonly Size Size;

        public uint EA { get; protected set; }

        // TODO: add operation length so we can skip addresses in disassembly

        public OpCode(MachineState state)
        {
            this.state = state;

            Address = state.PC;

            Size = getSize();

            validate();
        }

        protected void validate()
        {
            ushort count = 1;
            for (int i = 0; i < 16; i++)
            {
                switch (definition[15-i])
                {
                    case '0':
                        if ((state.OpCode & count) != 0x0000)
                        {
                            throw new InvalidOpCodeException();
                        }

                        break;

                    case '1':
                        if ((state.OpCode & count) == 0x0000)
                        {
                            throw new InvalidOpCodeException();
                        }

                        break;
                }

                count += count;
            }
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
                EA = displacement;
                return Size.Byte;
            }
        }

        protected Condition getCondition()
        {
            return (Condition)getBits('c');
        }

        protected bool checkCondition(uint ea)
        {
            switch (getCondition())
            {
                case Condition.NE:
                    return state.Condition_Z == false;

                default:
                    throw new InvalidStateException();
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

        protected EffectiveAddressMode decodeEAMode()
        {
            return decodeEAMode(getM(), getXn());
        }

        protected EffectiveAddressMode decodeEAMode(byte M, byte Xn)
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

        protected string getEAAssemblyString()
        {
            return getEAAssemblyString(decodeEAMode(), EA, getXn());
        }

        protected string getEAAssemblyString(EffectiveAddressMode mode, uint ea, byte xn)
        {
            switch (mode)
            {
                case EffectiveAddressMode.Immediate:
                    switch (Size)
                    {
                        case Size.Long:
                            return string.Format("#{0}", (int)ea);
                        case Size.Word:
                            return string.Format("#{0}", (short)ea);
                        default:
                            throw new NotImplementedException();
                    }

                case EffectiveAddressMode.AbsoluteWord:
                    return string.Format("0x{0:X4}", ea);

                case EffectiveAddressMode.AbsoluteLong:
                    return string.Format("0x{0:X8}", ea);

                case EffectiveAddressMode.AddressWithDisplacement:
                    return $"({(short)ea}, A{xn})";

                case EffectiveAddressMode.ProgramCounter_Displacement:
                    return $"({(short)ea}, PC)";

                case EffectiveAddressMode.Address:
                    return string.Format("(A{0})", xn);

                case EffectiveAddressMode.Address_PostIncremenet:
                    return string.Format("(A{0}+)", xn);

                case EffectiveAddressMode.DataRegister:
                    return string.Format("D{0}", xn);

                case EffectiveAddressMode.AddressRegister:
                    return string.Format("A{0}", xn);

                default:
                    throw new NotImplementedException(mode.ToString());
            }
        }

        protected string getEADescriptionString()
        {
            return getEADescriptionString(decodeEAMode(), EA, getXn());
        }

        protected string getEADescriptionString(EffectiveAddressMode mode, uint ea, byte xn)
        {
            switch (mode)
            {
                case EffectiveAddressMode.Immediate:
                    switch (Size)
                    {
                        case Size.Long:
                            return string.Format("#{0}", (int)ea);
                        case Size.Word:
                            return string.Format("#{0}", (short)ea);
                        default:
                            throw new NotImplementedException();
                    }

                case EffectiveAddressMode.AbsoluteWord:
                    return string.Format("0x{0:X4}", ea);

                case EffectiveAddressMode.AbsoluteLong:
                    return string.Format("0x{0:X8}", ea);

                case EffectiveAddressMode.AddressWithDisplacement:
                    return $"(d16, A{xn})";

                case EffectiveAddressMode.ProgramCounter_Displacement:
                    return $"(d16, PC)";

                case EffectiveAddressMode.Address:
                    return string.Format("(A{0})", xn);

                case EffectiveAddressMode.Address_PostIncremenet:
                    return string.Format("(A{0}+)", xn);

                case EffectiveAddressMode.DataRegister:
                    return string.Format("D{0}", xn);

                case EffectiveAddressMode.AddressRegister:
                    return string.Format("A{0}", xn);

                default:
                    throw new NotImplementedException(mode.ToString());
            }
        }

        protected uint getEAValue()
        {
            return getEAValue(decodeEAMode(), EA, getXn());
        }

        protected uint getEAValue(EffectiveAddressMode mode, uint ea, byte xn)
        {
            switch (mode)
            {
                case EffectiveAddressMode.Immediate:
                    return ea;

                case EffectiveAddressMode.AbsoluteWord:
                    switch (Size)
                    {
                        case Size.Long:
                            return state.ReadLong(ea);
                        case Size.Word:
                            return state.ReadWord(ea);
                        default:
                            throw new NotImplementedException();
                    }

                case EffectiveAddressMode.AbsoluteLong:
                    switch (Size)
                    {
                        case Size.Long:
                            return state.ReadLong(ea);
                        case Size.Word:
                            return state.ReadWord(ea);
                        default:
                            throw new NotImplementedException();
                    }

                case EffectiveAddressMode.DataRegister:
                    return state.ReadDReg((byte)ea);

                case EffectiveAddressMode.AddressRegister:
                    return state.ReadAReg((byte)ea);

                case EffectiveAddressMode.Address:
                    throw new NotImplementedException();
                //return state.Read(state.ReadAReg(xn));

                case EffectiveAddressMode.ProgramCounter_Displacement:
                    // TODO: is -4 required?
                    return (uint)(state.PC - 4 + (short)ea);

                default:
                    throw new NotImplementedException(mode.ToString());
            }
        }

        protected void setEAValue(EffectiveAddressMode ea, uint value)
        {
            setEAValue(ea, getXn(), value);
        }

        protected void setEAValue(EffectiveAddressMode ea, uint Xn, uint value)
        {
            switch (ea)
            {
                case EffectiveAddressMode.AbsoluteWord:
                    //                    state.Write(Xn + 0, (byte)((value >> 0) & 0xFF));
                    //                    state.Write(Xn + 1, (byte)((value >> 8) & 0xFF));
                    throw new NotImplementedException();
                    break;

                case EffectiveAddressMode.Address:
                    uint addr = state.ReadAReg((byte)Xn);

                    // TODO: check Size????

//                    state.Write(addr + 0, (byte)((value >> 0) & 0xFF));
//                    state.Write(addr + 1, (byte)((value >> 8) & 0xFF));
                    throw new NotImplementedException();

                    break;

                case EffectiveAddressMode.DataRegister:

                    // TODO: write all 32 bits

                    state.WriteDReg((byte)Xn, value);

                    break;

                case EffectiveAddressMode.AddressRegister:

                    // TODO: write all 32 bits

                    state.WriteAReg((byte)Xn, value);

                    break;

                default:
                    throw new NotImplementedException(ea.ToString());
            }
        }

        protected uint readEA()
        {
            return readEA(decodeEAMode(), getXn());
        }

        protected uint readEA(EffectiveAddressMode ea)
        {
            return readEA(ea, getXn());
        }

        protected uint readEA(EffectiveAddressMode ea, byte Xn)
        {
            switch (ea)
            {
                case EffectiveAddressMode.Immediate:
                    return (uint)readData(Size);

                case EffectiveAddressMode.AbsoluteWord:
                    return (uint)readData(Size.Word);

                case EffectiveAddressMode.AbsoluteLong:
                    return (uint)readData(Size.Long);

                case EffectiveAddressMode.DataRegister:
                    return Xn;

                case EffectiveAddressMode.AddressWithDisplacement:
                    return (uint)readData(Size.Word);

                case EffectiveAddressMode.ProgramCounter_Displacement:
                    return (uint)readData(Size.Word);

                case EffectiveAddressMode.Address:
                    return state.ReadAReg(Xn);

                case EffectiveAddressMode.AddressRegister:
                    return Xn;

                case EffectiveAddressMode.Address_PostIncremenet:
                    uint api = state.ReadAReg(Xn);
                    state.WriteAReg(Xn, (byte)((api + 1) & 0xFF));
                    return api;

                default:
                    throw new NotImplementedException(ea.ToString());
            }
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

        protected bool isNegative(uint val)
        {
            switch (Size)
            {
                case Size.Byte:
                    return ((sbyte)val) < 0;
                case Size.Word:
                    return ((short)val) < 0;
                case Size.Long:
                    return ((int)val) < 0;
                default:
                    throw new InvalidStateException();
            }
        }

        protected bool isZero(uint val)
        {
            switch (Size)
            {
                case Size.Byte:
                    return ((sbyte)val) == 0;
                case Size.Word:
                    return ((short)val) == 0;
                case Size.Long:
                    return ((int)val) == 0;
                default:
                    throw new InvalidStateException();
            }
        }
    }
}
