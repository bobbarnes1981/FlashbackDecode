namespace Decoder
{
    using System;
    using Decoder.Exceptions;

    /// <summary>
    /// Sega Mega Drive Machine State.
    /// </summary>
    public class MegadriveState
    {
        public static ConsoleWriter Writer = new ConsoleWriter();

        private Data rom;

        private Data ram68k;

        private uint[] AddressRegisters;

        private uint[] DataRegisters;

        private uint USP;

        private uint SSP;

        public uint SP
        {
            get
            {
                if (Condition_Supervisor)
                {
                    return SSP;
                }
                else
                {
                    return USP;
                }
            }

            set
            {
                if (Condition_Supervisor)
                {
                    SSP = value;
                }
                else
                {
                    USP = value;
                }
            }
        }

        public uint PC;

        public ushort OpCode { get; private set; }

        public ushort SR { get; set; }

        private uint ROM_MIN = 0x000000;
        private uint ROM_MAX = 0x3FFFFF;

        private uint RAM_MIN = 0xFF0000;
        private uint RAM_MAX = 0xFFFFFF;

        private uint Z80_MEM_MIN = 0xA00000;
        private uint Z80_MEM_MAX = 0xA0FFFF;

        public MegadriveState(Data rom, uint origin, uint sp, uint romMin, uint romMax, uint ramMin, uint ramMax)
        {
            this.PC = origin;
            this.USP = sp;

            this.ROM_MIN = romMin;
            this.ROM_MAX = romMax;

            this.RAM_MIN = ramMin;
            this.RAM_MAX = ramMax;

            this.rom = rom;
            this.ram68k = new Data(new byte[0x10000]);

            this.AddressRegisters = new uint[]
            {
                0x00,
                0x00,
                0x00,
                0x00,
                0x00,
                0x00,
                0x00,
            };

            this.DataRegisters = new uint[]
            {
                0x00,
                0x00,
                0x00,
                0x00,
                0x00,
                0x00,
                0x00,
                0x00,
            };
        }

        public void FetchOpCode()
        {
            this.OpCode = this.rom.ReadWord(PC);
            this.PC += 2;
        }

        /// <summary>
        /// Mega drive can only address 24 bit.
        /// </summary>
        /// <param name="address">address to mask.</param>
        /// <returns>address masked to 24 bit.</returns>
        private uint maskAddress(uint address)
        {
            return address & 0x00FFFFFF;
        }

        public byte ReadByte(uint address)
        {
            address = this.maskAddress(address);

            if (address <= this.ROM_MAX)
            {
                return this.rom.ReadByte(address);
            }

            if (address >= this.RAM_MIN && address <= this.RAM_MAX)
            {
                return this.ram68k.ReadByte(address - this.RAM_MIN);
            }

            if (address == 0xA10001)
            {
                Writer.Write("Reading second byte of version register", ConsoleColor.Yellow);
                return 0x00;
            }

            if (address == 0xA11100)
            {
                Writer.Write("Reading Z80 Bus Request", ConsoleColor.Yellow);
                return 0x00;
            }

            throw new NotImplementedException($"{address:X8}");
        }

        public void WriteByte(uint address, byte data)
        {
            address = this.maskAddress(address);

            if (address <= this.ROM_MAX)
            {
                throw new InvalidStateException();
            }
            else if (address >= this.Z80_MEM_MIN && address <= this.Z80_MEM_MAX)
            {
                Writer.Write("Writing to Z80 Memory", ConsoleColor.Yellow);
            }
            else if (address == 0xC00011)
            {
                Writer.Write("Writing to PSG Output", ConsoleColor.Yellow);
            }
            else if (address >= this.RAM_MIN && address <= this.RAM_MAX)
            {
                this.ram68k.WriteByte(address - this.RAM_MIN, data);
            }
            else
            {
                throw new NotImplementedException($"{address:X8}");
            }
        }

        public ushort ReadWord(uint address)
        {
            address = this.maskAddress(address);

            if (address <= this.ROM_MAX)
            {
                return this.rom.ReadWord(address);
            }

            if (address == 0x00A1000C)
            {
                Writer.Write("Reading Expansion Port Control", ConsoleColor.Yellow);
                return 0x0000;
            }

            if (address == 0x00C00004)
            {
                Writer.Write("Reading VDP Control Port", ConsoleColor.Yellow);
                return 0x0000;
            }

            if (address >= this.RAM_MIN && address <= this.RAM_MAX)
            {
                return this.ram68k.ReadWord(address - this.RAM_MIN);
            }

            throw new NotImplementedException($"{address:X8}");
        }

        public void WriteWord(uint address, ushort data)
        {
            address = this.maskAddress(address);

            if (address <= this.ROM_MAX)
            {
                throw new InvalidStateException();
            }
            else if (address == 0x00A11100)
            {
                Writer.Write("Writing Z80 Bus Request", ConsoleColor.Yellow);
            }
            else if (address == 0x00A11200)
            {
                Writer.Write("Writing Z80 Reset", ConsoleColor.Red);
            }
            else if (address == 0x00C00000)
            {
                Writer.Write("Writing VDP Data Port", ConsoleColor.Yellow);
            }
            else if (address == 0x00C00004)
            {
                Writer.Write("Writing VDP Control Port", ConsoleColor.Yellow);
            }
            else if (address >= this.RAM_MIN && address <= this.RAM_MAX)
            {
                this.ram68k.WriteWord(address - this.RAM_MIN, data);
            }
            else
            {
                throw new NotImplementedException($"{address:X8}");
            }
        }

        public uint ReadLong(uint address)
        {
            address = this.maskAddress(address);

            if (address <= this.ROM_MAX)
            {
                return this.rom.ReadLong(address);
            }

            if (address == 0xA10008)
            {
                Writer.Write("Reading Controller 1 Control and Controller 2 Control", ConsoleColor.Yellow);
                return 0x00000000;
            }

            if (address >= this.RAM_MIN && address <= this.RAM_MAX)
            {
                return this.ram68k.ReadLong(address - this.RAM_MIN);
            }

            throw new NotImplementedException($"{address:X8}");
        }

        public void WriteLong(uint address, uint data)
        {
            address = this.maskAddress(address);

            if (address <= this.ROM_MAX)
            {
                throw new InvalidStateException();
            }
            else if (address == 0x00C00000)
            {
                Writer.Write("Writing VDP Data Port", ConsoleColor.Yellow);
            }
            else if (address == 0x00C00004)
            {
                Writer.Write("Writing VDP Control Port (and mirror)", ConsoleColor.Yellow);
            }
            else if (address >= this.RAM_MIN && address <= this.RAM_MAX)
            {
                this.ram68k.WriteLong(address - this.RAM_MIN, data);
            }
            else
            {
                throw new NotImplementedException($"{address:X8}");
            }
        }

        public uint ReadAReg(byte register)
        {
            if (register > 0x07)
            {
                throw new Exception();
            }

            if (register == 0x07)
            {
                return (uint)this.SP;
            }
            else
            {
                return this.AddressRegisters[register];
            }
        }

        public void WriteAReg(byte register, uint data)
        {
            // TODO: sign extend data
            if (register > 0x07)
            {
                throw new Exception();
            }

            if (register == 0x07)
            {
                this.SP = data;
            }
            else
            {
                this.AddressRegisters[register] = data;
            }
        }

        public uint ReadDReg(byte register)
        {
            return this.DataRegisters[register];
        }

        public void WriteDReg(byte register, uint data)
        {
            this.DataRegisters[register] = data;
        }

        private const int CONDITION_TRACE = 13;
        private const int CONDITION_SUPERVISOR = 15;
        private const int CONDITION_X = 4;
        private const int CONDITION_NEGATIVE = 3;
        private const int CONDITION_ZERO = 2;
        private const int CONDITION_V = 1;
        private const int CONDITION_CARRY = 0;

        public bool Condition_X
        {
            get
            {
                return this.SR.GetBits(CONDITION_X, 1) == 0x1;
            }
            set
            {
                this.SR = this.SR.SetBits(CONDITION_X, 1, value);
            }
        }

        /// <summary>
        /// Negative.
        /// </summary>
        public bool Condition_N
        {
            get
            {
                return this.SR.GetBits(CONDITION_NEGATIVE, 1) == 0x1;
            }
            set
            {
                this.SR = this.SR.SetBits(CONDITION_NEGATIVE, 1, value);
            }
        }

        /// <summary>
        /// Zero.
        /// </summary>
        public bool Condition_Z
        {
            get
            {
                return this.SR.GetBits(CONDITION_ZERO, 1) == 0x1;
            }

            set
            {
                this.SR = this.SR.SetBits(CONDITION_ZERO, 1, value);
            }
        }

        /// <summary>
        /// Overflow.
        /// </summary>
        public bool Condition_V
        {
            get
            {
                return this.SR.GetBits(CONDITION_V, 1) == 0x1;
            }

            set
            {
                this.SR = this.SR.SetBits(CONDITION_V, 1, value);
            }
        }

        /// <summary>
        /// Carry.
        /// </summary>
        public bool Condition_C
        {
            get
            {
                return this.SR.GetBits(CONDITION_CARRY, 1) == 0x1;
            }

            set
            {
                this.SR = this.SR.SetBits(CONDITION_CARRY, 1, value);
            }
        }

        /// <summary>
        /// Trace enabled.
        /// </summary>
        public bool Condition_Trace
        {
            get
            {
                return this.SR.GetBits(CONDITION_TRACE, 1) == 0x1;
            }

            set
            {
                this.SR = this.SR.SetBits(CONDITION_TRACE, 1, value);
            }
        }

        /// <summary>
        /// Supervisor mode.
        /// </summary>
        public bool Condition_Supervisor
        {
            get
            {
                return this.SR.GetBits(CONDITION_SUPERVISOR, 1) == 0x1;
            }

            set
            {
                this.SR = this.SR.SetBits(CONDITION_SUPERVISOR, 1, value);
            }
        }
    }
}
