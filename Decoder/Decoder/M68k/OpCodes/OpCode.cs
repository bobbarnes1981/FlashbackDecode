namespace Decoder.M68k.OpCodes
{
    using Decoder.Exceptions;
    using Decoder.M68k.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public abstract Size Size { get; }

        /// <summary>
        /// Gets or sets the OpCode effective address.
        /// </summary>
        public uint EffectiveAddress { get; protected set; }

        /// <summary>
        /// Machine state.
        /// </summary>
        protected readonly MegadriveState state;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="OpCode"/> class.
        /// </summary>
        /// <param name="definition">opcode definition string.</param>
        /// <param name="state">machine state.</param>
        public OpCode(string definition, MegadriveState state)
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

            this.ValidateDefinition();
        }

        /// <summary>
        /// Validate the opcode against the definition string.
        /// </summary>
        private void ValidateDefinition()
        {
            ushort count = 1;
            for (int i = 0; i < 16; i++)
            {
                switch (this.definition[15 - i])
                {
                    case '0':
                        if ((this.state.OpCode & count) != 0x0000)
                        {
                            throw new InvalidOpCodeException();
                        }

                        break;

                    case '1':
                        if ((this.state.OpCode & count) == 0x0000)
                        {
                            throw new InvalidOpCodeException();
                        }

                        break;
                }

                count += count;
            }
        }

        protected void ValidateSize()
        {

        }

        protected void ValidateEffectiveAddress(EffectiveAddressMode mode, IEnumerable<EffectiveAddressMode> allowed)
        {
            if (allowed.Any(m => m == mode) == false)
            {
                throw new InvalidOpCodeException();
            }
        }

        /// <summary>
        /// Decode the size from the 1 bit immediate value.
        /// </summary>
        /// <param name="offset">bit offset.</param>
        /// <returns>size.</returns>
        protected Size GetSizeFrom1BitImmediate(int offset)
        {
            switch (this.state.OpCode.GetBits(offset, 1))
            {
                case 0x0000:
                    return Size.Word;
                case 0x0001:
                    return Size.Long;
                default:
                    throw new InvalidStateException();
            }
        }

        /// <summary>
        /// Decode the size from the 8 bit immediate value.
        /// </summary>
        /// <returns>size.</returns>
        protected Size GetSizeFrom8BitImmediate()
        {
            byte displacement = (byte)this.GetImmediate();

            if (displacement == 0x00)
            {
                return Size.Word;
            }
            else
            {
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

                case Condition.EQ:
                    return this.state.Condition_Z == true;

                default:
                    throw new InvalidStateException();
            }
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
            return (uint)this.ReadDataUsingPC(this.Size);
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

        protected string GetAssemblyForEffectiveAddress()
        {
            return GetAssemblyForEffectiveAddress(DecodeEffectiveAddressMode(), EffectiveAddress, GetXn());
        }

        protected string GetAssemblyForEffectiveAddress(EffectiveAddressMode mode, uint ea, byte xn)
        {
            switch (mode)
            {
                case EffectiveAddressMode.Immediate:
                    switch (this.Size)
                    {
                        case Size.Long:
                            return $"#{(int)ea}";
                        case Size.Word:
                            return $"#{(short)ea}";
                        case Size.Byte:
                            return $"#{(byte)ea}";
                        default:
                            throw new NotImplementedException();
                    }

                //case EffectiveAddressMode.AbsoluteWord:
                //    return string.Format("0x{0:X4}", ea);

                case EffectiveAddressMode.AbsoluteLong:
                    return $"${ea:X8}";

                case EffectiveAddressMode.AddressWithDisplacement:
                    return $"(${(short)ea:X4},A{xn})";

                case EffectiveAddressMode.ProgramCounterWithDisplacement:
                    return $"(${(short)ea:X4},PC)";

                case EffectiveAddressMode.Address:
                    return $"(A{xn})";

                case EffectiveAddressMode.AddressPostIncrement:
                    return $"(A{xn})+";

                case EffectiveAddressMode.AddressPreDecrement:
                    return $"-(A{xn})";

                case EffectiveAddressMode.DataRegister:
                    return $"D{xn}";

                case EffectiveAddressMode.AddressRegister:
                    return $"A{xn}";

                default:
                    throw new NotImplementedException(mode.ToString());
            }
        }

        protected string DescribeEffectiveAddress()
        {
            return DescribeEffectiveAddress(DecodeEffectiveAddressMode(), EffectiveAddress, GetXn());
        }

        protected string DescribeEffectiveAddress(EffectiveAddressMode mode, uint ea, byte xn)
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

                case EffectiveAddressMode.ProgramCounterWithDisplacement:
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

        protected uint ResolveEffectiveAddress()
        {
            return this.ResolveEffectiveAddress(this.DecodeEffectiveAddressMode(), this.EffectiveAddress, this.GetXn());
        }

        protected uint ResolveEffectiveAddress(EffectiveAddressMode mode, uint ea, byte xn)
        {
            switch (mode)
            {
                // get the address stored in the address register
                case EffectiveAddressMode.Address:
                    return this.state.ReadAReg((byte)xn);

                // get the address stored in the address register plus the displacement
                case EffectiveAddressMode.AddressWithDisplacement:
                    return this.state.ReadAReg((byte)xn) + ea;

                // get the address stored in the program counter plus the displacement
                case EffectiveAddressMode.ProgramCounterWithDisplacement:
                    return this.state.PC + ea - 2;

                default:
                    throw new InvalidStateException();
            }
        }

        protected uint InterpretEffectiveAddress()
        {
            return InterpretEffectiveAddress(DecodeEffectiveAddressMode(), EffectiveAddress, GetXn());
        }

        protected uint InterpretEffectiveAddress(EffectiveAddressMode mode, uint ea, byte xn)
        {
            switch (mode)
            {
                // get the immediate value
                case EffectiveAddressMode.Immediate:
                    return ea;

                // get the data in memory pointed to by the address register
                case EffectiveAddressMode.Address:
                    switch (this.Size)
                    {
                        case Size.Long:
                            return this.state.ReadLong(this.state.ReadAReg(xn));
                        case Size.Word:
                            return this.state.ReadWord(this.state.ReadAReg(xn));
                        case Size.Byte:
                            return this.state.ReadByte(this.state.ReadAReg(xn));
                        default:
                            throw new InvalidStateException();
                    }

                // get the data in memory pointed to by the address register (post increment register)
                case EffectiveAddressMode.AddressPostIncrement:
                    switch (this.Size)
                    {
                        case Size.Long:
                            var regl = this.state.ReadAReg(xn);
                            var l = this.state.ReadLong(regl);
                            this.state.WriteAReg(xn, regl + 4);
                            return l;
                        case Size.Byte:
                            var regb = this.state.ReadAReg(xn);
                            var b = this.state.ReadByte(regb);
                            this.state.WriteAReg(xn, regb + 4);
                            return b;
                        default:
                            throw new InvalidStateException();
                    }

                // get the data in memory pointed to by the address register (pre decrement register)
                case EffectiveAddressMode.AddressPreDecrement:
                    switch (this.Size)
                    {
                        case Size.Long:
                            var regl = this.state.ReadAReg(xn);
                            regl -= 4;
                            var l = this.state.ReadLong(regl);
                            this.state.WriteAReg(xn, regl);
                            return l;
                        case Size.Byte:
                            var regb = this.state.ReadAReg(xn);
                            regb -= 2;
                            var b = this.state.ReadByte(regb);
                            this.state.WriteAReg(xn, regb);
                            return b;
                        default:
                            throw new InvalidStateException();
                    }

                // get the number stored in the address register
                case EffectiveAddressMode.AddressRegister:
                    return this.state.ReadAReg(xn);

                // get the number stored in the data register
                case EffectiveAddressMode.DataRegister:
                    return this.state.ReadDReg(xn);

                // get the value from memory using the provided long address
                case EffectiveAddressMode.AbsoluteLong:
                    switch (this.Size)
                    {
                        case Size.Long:
                            return this.state.ReadLong(ea);
                        case Size.Word:
                            return this.state.ReadWord(ea);
                        default:
                            throw new InvalidStateException();
                    }

                // get the data addressed by the contents of the address register + displacement
                case EffectiveAddressMode.AddressWithDisplacement:
                    var ad = (uint)(this.state.ReadAReg(xn) + (short)ea);
                    switch (this.Size)
                    {
                        case Size.Long:
                            return this.state.ReadLong(ad);
                        case Size.Word:
                            return this.state.ReadWord(ad);
                        case Size.Byte:
                            return this.state.ReadByte(ad);
                        default:
                            throw new InvalidStateException();
                    }

                // get the data addressed by PC + displacement (-2 so it is from opcode address)
                case EffectiveAddressMode.ProgramCounterWithDisplacement:
                    var pcd = (uint)(this.state.PC - 2 + (short)ea);
                    switch (this.Size)
                    {
                        case Size.Long:
                            return this.state.ReadLong(pcd);
                        case Size.Word:
                            return this.state.ReadWord(pcd);
                        case Size.Byte:
                            return this.state.ReadByte(pcd);
                        default:
                            throw new InvalidStateException();
                    }

                default:
                    throw new NotImplementedException(mode.ToString());
            }
        }

        protected void WriteValueToEffectiveAddress(EffectiveAddressMode ea, uint Xn, uint value)
        {
            switch (ea)
            {
                // write the value at the address to the specified data register
                case EffectiveAddressMode.DataRegister:
                    switch (this.Size)
                    {
                        case Size.Byte:
                            this.state.WriteDReg((byte)Xn, (uint)(byte)value);
                            break;
                        case Size.Word:
                            this.state.WriteDReg((byte)Xn, (uint)(short)value);
                            break;
                        case Size.Long:
                            this.state.WriteDReg((byte)Xn, (uint)value);
                            break;
                        default:
                            throw new InvalidStateException();
                    }

                    break;

                // write the value to the specified address register
                case EffectiveAddressMode.AddressRegister:
                    this.state.WriteAReg((byte)Xn, value);
                    break;

                // write the value to the specified address register
                case EffectiveAddressMode.AddressPostIncrement:
                    this.state.WriteAReg((byte)Xn, value);
                    break;

                // write value to address specified in An
                case EffectiveAddressMode.Address:
                    switch (this.Size)
                    {
                        case Size.Byte:
                            this.state.WriteByte(this.state.ReadAReg((byte)Xn), (byte)value);
                            break;
                        case Size.Word:
                            throw new NotImplementedException();
                        case Size.Long:
                            throw new NotImplementedException();
                        default:
                            throw new InvalidStateException();
                    }

                    break;

                default:
                    throw new NotImplementedException(ea.ToString());
            }
        }

        protected uint FetchEffectiveAddress()
        {
            return this.FetchEffectiveAddress(DecodeEffectiveAddressMode(), GetXn());
        }

        protected uint FetchEffectiveAddress(EffectiveAddressMode ea, byte Xn)
        {
            switch (ea)
            {
                // get the address data immediately following the opcode
                case EffectiveAddressMode.AbsoluteLong:
                    return (uint)this.ReadDataUsingPC(Size.Long);

                // get the address data immediately following the opcode
                case EffectiveAddressMode.AbsoluteWord:
                    return (uint)this.ReadDataUsingPC(Size.Long);

                // get the immediate value follwoing the opcode
                case EffectiveAddressMode.Immediate:
                    return (uint)this.ReadDataUsingPC(this.Size);

                // get the address register number
                case EffectiveAddressMode.Address:
                    return Xn;

                // get the address register number
                case EffectiveAddressMode.AddressRegister:
                    return Xn;

                // get the data register number
                case EffectiveAddressMode.DataRegister:
                    return Xn;

                // get the displacement word immediately following the opcode
                case EffectiveAddressMode.AddressWithDisplacement:
                    return (uint)this.ReadDataUsingPC(Size.Word);

                // get the displacement word immediately following the opcode
                case EffectiveAddressMode.ProgramCounterWithDisplacement:
                    return (uint)this.ReadDataUsingPC(Size.Word);

                // get the reg value
                case EffectiveAddressMode.AddressPostIncrement:
                    switch (this.Size)
                    {
                        case Size.Byte:
                            return this.state.ReadAReg(Xn);
                        case Size.Word:
                            return this.state.ReadAReg(Xn);
                        case Size.Long:
                            return this.state.ReadAReg(Xn);
                        default:
                            throw new InvalidStateException();
                    }

                // get the reg value
                case EffectiveAddressMode.AddressPreDecrement:
                    switch (this.Size)
                    {
                        case Size.Byte:
                            return this.state.ReadAReg(Xn);
                        case Size.Word:
                            return this.state.ReadAReg(Xn);
                        case Size.Long:
                            return this.state.ReadAReg(Xn);
                        default:
                            throw new InvalidStateException();
                    }

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
        protected int ReadDataUsingPC(Size size)
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
                case Size.Byte:
                    // read word but discard first byte
                    this.state.ReadByte(this.state.PC);
                    this.state.PC += 1;
                    byte b = (byte)this.state.ReadByte(this.state.PC);
                    this.state.PC += 1;
                    return (int)b;
                    throw new NotImplementedException();
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
    }
}
