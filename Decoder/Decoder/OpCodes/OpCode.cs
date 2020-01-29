namespace Decoder.OpCodes
{
    using System;

    /// <summary>
    /// OpCode base class.
    /// </summary>
    public abstract class OpCode
    {
        /// <summary>
        /// Gets the address of the OpCode.
        /// </summary>
        public readonly uint Address;

        /// <summary>
        /// Gets the OpCode name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the OpCode description.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the OpCode operation.
        /// </summary>
        public abstract string Operation { get; }

        /// <summary>
        /// Gets the OpCode syntax.
        /// </summary>
        public abstract string Syntax { get; }

        /// <summary>
        /// Gets the OpCode assembly instruction.
        /// </summary>
        public abstract string Assembly { get; }

        /// <summary>
        /// Gets the OpCode size.
        /// </summary>
        public readonly Size Size;

        /// <summary>
        /// Gets the OpCode effective address.
        /// </summary>
        public uint EffectiveAddress { get; protected set; }

        protected readonly MachineState state;

        /// <summary>
        /// Gets the OpCode definition string.
        /// </summary>
        private readonly string definition;

        private const char DEFINITION_CHAR_MODE = 'm';
        private const char DEFINITION_CHAR_XN = 'x';
        private const char DEFINITION_CHAR_IMMEDIATE = 'b';
        private const char DEFINITION_CHAR_DN = 'd';
        private const char DEFINITION_CHAR_AN = 'a';
        private const char DEFINITION_CHAR_CONDITION = 'c';

        public OpCode(string definition, MachineState state)
        {
            if (string.IsNullOrEmpty(definition))
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (definition.Length != 16)
            {
                throw new Exception("definition should be 16 characters.");
            }

            this.definition = definition;

            this.state = state;

            this.Address = state.PC;

            this.Size = this.getSize();

            this.validate();
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

        protected Size getSizeFrom1Bit(int offset)
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
            byte displacement = (byte)GetImmediate();

            if (displacement == 0x00)
            {
                return Size.Word;
            }
            else
            {
                EffectiveAddress = displacement;
                return Size.Byte;
            }
        }

        /// <summary>
        /// Check the status register for the opcode condition.
        /// </summary>
        /// <returns>true if the status register meets the opcode condition.</returns>
        protected bool CheckCondition()
        {
            switch (this.GetCondition())
            {
                case Condition.NE:
                    return this.state.Condition_Z == false;

                default:
                    throw new InvalidStateException();
            }
        }

        /// <summary>
        /// Get the offset and length values defined by specified character in definition.
        /// </summary>
        /// <param name="c">character to search for in definition.</param>
        /// <returns>offset and length values.</returns>
        private Tuple<int, int> GetValues(char c)
        {
            int offset = -1;
            int length = -1;
            for (int i = 0; i < 16; i++)
            {
                if (this.definition[15 - i] == c)
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
                throw new Exception($"{c} not defined");
            }

            return new Tuple<int, int>(offset, length);
        }

        /// <summary>
        /// Get bits from opcode defined by specified character in definition.
        /// </summary>
        /// <param name="c">character to search for in definition.</param>
        /// <returns>ushort representing the data.</returns>
        protected ushort GetBits(char c)
        {
            var vals = this.GetValues(c);
            return this.state.OpCode.GetBits(vals.Item1, vals.Item2);
        }

        /// <summary>
        /// Fetch the condition data from the definition.
        /// Identified by 'c' in definition.
        /// </summary>
        /// <returns>condition enum</returns>
        protected Condition GetCondition()
        {
            return (Condition)this.GetBits(DEFINITION_CHAR_CONDITION);
        }

        /// <summary>
        /// Fetch the address register number from the definition.
        /// Identified by 'a' in definition.
        /// </summary>
        /// <returns>address register enum (castable to byte)</returns>
        protected AddressRegister GetAn()
        {
            return (AddressRegister)this.GetBits(DEFINITION_CHAR_AN);
        }

        /// <summary>
        /// Fetch the data register number from the definition.
        /// Identified by 'd' in definition.
        /// </summary>
        /// <returns>data register enum (castable to byte).</returns>
        protected DataRegister GetDn()
        {
            return (DataRegister)this.GetBits(DEFINITION_CHAR_DN);
        }

        /// <summary>
        /// Fetch the immediate data from the definition.
        /// Ientified by b in definition.
        /// </summary>
        /// <returns>ushort representing immediate data.</returns>
        protected ushort GetImmediate()
        {
            return this.GetBits(DEFINITION_CHAR_IMMEDIATE);
        }

        /// <summary>
        /// Fetch the Mode data M from the definition.
        /// Identified by 'm' in definition.
        /// </summary>
        /// <returns>byte representing M.</returns>
        protected byte GetM()
        {
            return (byte)this.GetBits(DEFINITION_CHAR_MODE);
        }

        /// <summary>
        /// Fetch the Address, Data register or address mode number Xn from the definition.
        /// Identified by 'x' in definition.
        /// </summary>
        /// <returns>byte representing Xn.</returns>
        protected byte GetXn()
        {
            return (byte)this.GetBits(DEFINITION_CHAR_XN);
        }

        /// <summary>
        /// Read immediate data from memory using the program counter.
        /// Program counter will be incremented.
        /// This is normally data immediately after the opcode.
        /// </summary>
        /// <returns>data from memory.</returns>
        protected uint ReadImmediate()
        {
            return (uint)this.ReadData(this.Size);
        }

        /// <summary>
        /// Decode the Effective Address Mode from the definition.
        /// Decoded from M and Xn.
        /// </summary>
        /// <returns>Effective Address enum.</returns>
        protected EffectiveAddressMode DecodeEffectiveAddressMode()
        {
            return this.DecodeEffectiveAddressMode(this.GetM(), this.GetXn());
        }

        /// <summary>
        /// Decode the Effective Address Mode from the definition.
        /// Decoded from M and Xn.
        /// </summary>
        /// <param name="m">M parameter.</param>
        /// <param name="xn">Xn parameter.</param>
        /// <returns>Effective Address enum.</returns>
        protected EffectiveAddressMode DecodeEffectiveAddressMode(byte m, byte xn)
        {
            // TODO: validate allowed addressing modes
            if (m < 0x07)
            {
                return (EffectiveAddressMode)(m << 3);
            }
            else
            {
                return (EffectiveAddressMode)(m << 3 | xn);
            }
        }

        protected string getEAAssemblyString()
        {
            return getEAAssemblyString(DecodeEffectiveAddressMode(), EffectiveAddress, GetXn());
        }

        protected string getEAAssemblyString(EffectiveAddressMode mode, uint ea, byte xn)
        {
            switch (mode)
            {
                //case EffectiveAddressMode.Immediate:
                //    switch (Size)
                //    {
                //        case Size.Long:
                //            return string.Format("#{0}", (int)ea);
                //        case Size.Word:
                //            return string.Format("#{0}", (short)ea);
                //        default:
                //            throw new NotImplementedException();
                //    }

                //case EffectiveAddressMode.AbsoluteWord:
                //    return string.Format("0x{0:X4}", ea);

                case EffectiveAddressMode.AbsoluteLong:
                    return $"0x{ea:X8}";

                case EffectiveAddressMode.AddressWithDisplacement:
                    return $"({(short)ea}, A{xn})";

                case EffectiveAddressMode.ProgramCounter_Displacement:
                    return $"(0x{(short)ea:X4}, PC)";

                //case EffectiveAddressMode.Address:
                //    return string.Format("(A{0})", xn);

                case EffectiveAddressMode.Address_PostIncremenet:
                    return $"(A{xn})+";

                case EffectiveAddressMode.DataRegister:
                    return $"D{xn}";

                //case EffectiveAddressMode.AddressRegister:
                //    return string.Format("A{0}", xn);

                default:
                    throw new NotImplementedException(mode.ToString());
            }
        }

        protected string getEADescriptionString()
        {
            return getEADescriptionString(DecodeEffectiveAddressMode(), EffectiveAddress, GetXn());
        }

        protected string getEADescriptionString(EffectiveAddressMode mode, uint ea, byte xn)
        {
            switch (mode)
            {
                //case EffectiveAddressMode.Immediate:
                //    switch (Size)
                //    {
                //        case Size.Long:
                //            return string.Format("#{0}", (int)ea);
                //        case Size.Word:
                //            return string.Format("#{0}", (short)ea);
                //        default:
                //            throw new NotImplementedException();
                //    }

                //case EffectiveAddressMode.AbsoluteWord:
                //    return string.Format("0x{0:X4}", ea);

                //case EffectiveAddressMode.AbsoluteLong:
                //    return string.Format("0x{0:X8}", ea);

                //case EffectiveAddressMode.AddressWithDisplacement:
                //    return $"(d16, A{xn})";

                case EffectiveAddressMode.ProgramCounter_Displacement:
                    return $"(d16, PC)";

                //case EffectiveAddressMode.Address:
                //    return string.Format("(A{0})", xn);

                //case EffectiveAddressMode.Address_PostIncremenet:
                //    return string.Format("(A{0}+)", xn);

                //case EffectiveAddressMode.DataRegister:
                //    return string.Format("D{0}", xn);

                //case EffectiveAddressMode.AddressRegister:
                //    return string.Format("A{0}", xn);

                default:
                    throw new NotImplementedException(mode.ToString());
            }
        }

        protected uint getEAValue()
        {
            return getEAValue(DecodeEffectiveAddressMode(), EffectiveAddress, GetXn());
        }

        protected uint getEAValue(EffectiveAddressMode mode, uint ea, byte xn)
        {
            switch (mode)
            {
                //case EffectiveAddressMode.Immediate:
                //    return ea;

                //case EffectiveAddressMode.AbsoluteWord:
                //    switch (Size)
                //    {
                //        case Size.Long:
                //            return state.ReadLong(ea);
                //        case Size.Word:
                //            return state.ReadWord(ea);
                //        default:
                //            throw new NotImplementedException();
                //    }

                case EffectiveAddressMode.AbsoluteLong:
                    switch (this.Size)
                    {
                        case Size.Long:
                            return this.state.ReadLong(ea);
                        case Size.Word:
                            return state.ReadWord(ea);
                        default:
                            throw new NotImplementedException();
                    }

                //case EffectiveAddressMode.DataRegister:
                //    return state.ReadDReg((byte)ea);

                //case EffectiveAddressMode.AddressRegister:
                //    return state.ReadAReg((byte)ea);

                //case EffectiveAddressMode.Address:
                //    throw new NotImplementedException();
                ////return state.Read(state.ReadAReg(xn));

                // get the contents of the address register + displacement
                case EffectiveAddressMode.AddressWithDisplacement:

                    return this.state.ReadByte((uint)(this.state.ReadAReg(xn) + (short)ea));
                // get the PC + displacement (-2 so it is from opcode address)
                case EffectiveAddressMode.ProgramCounter_Displacement:
                    return (uint)(this.state.PC - 2 + (short)ea);

                default:
                    throw new NotImplementedException(mode.ToString());
            }
        }

        protected void setEAValue(EffectiveAddressMode ea, uint value)
        {
            setEAValue(ea, GetXn(), value);
        }

        protected void setEAValue(EffectiveAddressMode ea, uint Xn, uint value)
        {
            switch (ea)
            {
                //                case EffectiveAddressMode.AbsoluteWord:
                //                    //                    state.Write(Xn + 0, (byte)((value >> 0) & 0xFF));
                //                    //                    state.Write(Xn + 1, (byte)((value >> 8) & 0xFF));
                //                    throw new NotImplementedException();
                //                    break;

                //                case EffectiveAddressMode.Address:
                //                    uint addr = state.ReadAReg((byte)Xn);

                //                    // TODO: check Size????

                ////                    state.Write(addr + 0, (byte)((value >> 0) & 0xFF));
                ////                    state.Write(addr + 1, (byte)((value >> 8) & 0xFF));
                //                    throw new NotImplementedException();

                //                    break;

                // write the value to the specified data register
                case EffectiveAddressMode.DataRegister:
                    this.state.WriteDReg((byte)Xn, (byte)value);
                    break;

                // write the value to the specified address register
                case EffectiveAddressMode.AddressRegister:
                    this.state.WriteAReg((byte)Xn, value);
                    break;

                // write the value to the specified address register
                case EffectiveAddressMode.Address_PostIncremenet:
                    this.state.WriteAReg((byte)Xn, value);
                    break;
                
                default:
                    throw new NotImplementedException(ea.ToString());
            }
        }

        protected uint readEA()
        {
            return readEA(DecodeEffectiveAddressMode(), GetXn());
        }

        protected uint readEA(EffectiveAddressMode ea)
        {
            return readEA(ea, GetXn());
        }

        protected uint readEA(EffectiveAddressMode ea, byte Xn)
        {
            switch (ea)
            {
                //case EffectiveAddressMode.Immediate:
                //    return (uint)ReadData(Size);

                //case EffectiveAddressMode.AbsoluteWord:
                //    return (uint)ReadData(Size.Word);

                case EffectiveAddressMode.AbsoluteLong:
                    return (uint)this.ReadData(Size.Long);

                // get the data register number
                case EffectiveAddressMode.DataRegister:
                    return Xn;

                // get the displacement word immediately following the opcode
                case EffectiveAddressMode.AddressWithDisplacement:
                    return (uint)this.ReadData(Size.Word);

                // get the displacement word immediately following the opcode
                case EffectiveAddressMode.ProgramCounter_Displacement:
                    return (uint)this.ReadData(Size.Word);

                //case EffectiveAddressMode.Address:
                //    return state.ReadAReg(Xn);

                //case EffectiveAddressMode.AddressRegister:
                //    return Xn;

                // get the reg value and increment
                case EffectiveAddressMode.Address_PostIncremenet:
                    uint api = this.state.ReadAReg(Xn);
                    this.state.WriteAReg(Xn, api + 1);
                    return api;

                default:
                    throw new NotImplementedException(ea.ToString());
            }
        }

        /// <summary>
        /// Reads data of the specified size from memory using the program counter
        /// and incrementing the program counter.
        /// </summary>
        /// <param name="size">Size of data to read.</param>
        /// <returns>The read data cast to int.</returns>
        protected int ReadData(Size size)
        {
            switch (size)
            {
                case Size.Long:
                    uint l = this.state.ReadLong(this.state.PC);
                    this.state.PC += 4;
                    return (int)l;
                case Size.Word:
                    ushort w = this.state.ReadWord(this.state.PC);
                    this.state.PC += 2;
                    return (int)w;
                //case Size.Byte:
                //    // read word but discard first byte
                //    this.state.ReadByte(this.state.PC);
                //    this.state.PC += 1;
                //    byte b = (byte)this.state.ReadByte(this.state.PC);
                //    this.state.PC += 1;
                //    return (int)b;
                default:
                    throw new InvalidStateException();
            }
        }

        /// <summary>
        /// Returns true if the specified value is negative when cast to the operand Size.
        /// </summary>
        /// <param name="val">value to check.</param>
        /// <returns>true if negative.</returns>
        protected bool IsNegative(uint val)
        {
            switch (this.Size)
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

        /// <summary>
        /// Returns true if the specified valie is zero when cast to the operand size.
        /// </summary>
        /// <param name="val">value to check.</param>
        /// <returns>true if zero.</returns>
        protected bool IsZero(uint val)
        {
            switch (this.Size)
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
