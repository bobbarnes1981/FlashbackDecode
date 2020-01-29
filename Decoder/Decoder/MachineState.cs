using System;

namespace Decoder
{
    /// <summary>
    /// Sega Mega Drive Machine State.
    /// </summary>
    public class MachineState
    {
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

        public MachineState(Data rom, uint origin, uint sp, uint romMin, uint romMax, uint ramMin, uint ramMax)
        {
            PC = origin;
            USP = sp;

            ROM_MIN = romMin;
            ROM_MAX = romMax;

            RAM_MIN = ramMin;
            RAM_MAX = ramMax;

            this.rom = rom;
            ram68k = new Data(new byte[0x10000]);

            AddressRegisters = new uint[]
            {
                0x00,
                0x00,
                0x00,
                0x00,
                0x00,
                0x00,
                0x00,
            };

            DataRegisters = new uint[]
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
            OpCode = rom.ReadWord(PC);
            PC += 2;
        }

        public byte ReadByte(uint address)
        {
            if (address <= ROM_MAX)
            {
                return rom.ReadByte(address);
            }

            throw new NotImplementedException();
        }

        public void WriteByte(uint address, byte data)
        {
            if (address <= ROM_MAX)
            {
                throw new InvalidStateException();
            }
            else if (address >= RAM_MIN && address <= RAM_MAX)
            {
                ram68k.WriteByte(address - RAM_MIN, data);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public ushort ReadWord(uint address)
        {
            if (address <= ROM_MAX)
            {
                return rom.ReadWord(address);
            }

            if (address == 0x00A1000C)
            {
                Console.WriteLine("Reading Expansion Port Control");
                return 0x0000;
            }

            if (address >= RAM_MIN && address <= RAM_MAX)
            {
                return ram68k.ReadWord(address - RAM_MIN);
            }

            throw new NotImplementedException($"{address:X8}");
        }

        public uint ReadLong(uint address)
        {
            if (address <= ROM_MAX)
            {
                return rom.ReadLong(address);
            }

            if (address == 0xA10008)
            {
                Console.WriteLine("Reading Controller 1 Control and Controller 2 Control");
                return 0x00000000;
            }

            if (address >= RAM_MIN && address <= RAM_MAX)
            {
                return ram68k.ReadLong(address - RAM_MIN);
            }

            throw new NotImplementedException($"{address:X8}");
        }

        public uint ReadAReg(byte register)
        {
            if (register > 0x07)
            {
                throw new System.Exception();
            }

            if (register == 0x07)
            {
                return (uint)SP;
            }
            else
            {
                return AddressRegisters[register];
            }
        }

        public void WriteAReg(byte register, uint data)
        {
            if (register > 0x07)
            {
                throw new System.Exception();
            }

            if (register == 0x07)
            {
                SP = data;
            }
            else
            {
                AddressRegisters[register] = data;
            }
        }

        public uint ReadDReg(byte register)
        {
            return DataRegisters[register];
        }

        public void WriteDReg(byte register, uint data)
        {
            DataRegisters[register] = data;
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
                return SR.GetBits(CONDITION_X, 1) == 0x1;
            }
            set
            {
                SR = SR.SetBits(CONDITION_X, 1, value);
            }
        }

        public bool Condition_N
        {
            get
            {
                return SR.GetBits(CONDITION_NEGATIVE, 1) == 0x1;
            }
            set
            {
                SR = SR.SetBits(CONDITION_NEGATIVE, 1, value);
            }
        }

        public bool Condition_Z
        {
            get
            {
                return SR.GetBits(CONDITION_ZERO, 1) == 0x1;
            }
            set
            {
                SR = SR.SetBits(CONDITION_ZERO, 1, value);
            }
        }

        public bool Condition_V
        {
            get
            {
                return SR.GetBits(CONDITION_V, 1) == 0x1;
            }
            set
            {
                SR = SR.SetBits(CONDITION_V, 1, value);
            }
        }

        public bool Condition_C
        {
            get
            {
                return SR.GetBits(CONDITION_CARRY, 1) == 0x1;
            }
            set
            {
                SR = SR.SetBits(CONDITION_CARRY, 1, value);
            }
        }

        public bool Condition_Trace
        {
            get
            {
                return SR.GetBits(CONDITION_TRACE, 1) == 0x1;
            }
            set
            {
                SR = SR.SetBits(CONDITION_TRACE, 1, value);
            }
        }

        public bool Condition_Supervisor
        {
            get
            {
                return SR.GetBits(CONDITION_SUPERVISOR, 1) == 0x1;
            }
            set
            {
                SR = SR.SetBits(CONDITION_SUPERVISOR, 1, value);
            }
        }
    }
}
