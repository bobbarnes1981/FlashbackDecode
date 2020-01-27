namespace Decoder
{
    class MachineState
    {
        private Data rom;

        private Data memory;

        private uint[] AddressRegisters;

        private uint[] DataRegisters;

        private int USP;

        private int SSP;

        public int SP
        {
            get
            {
                // TODO: check supervisor bit
                return USP;
            }
            set
            {
                // TODO: check supervisor bit
                USP = value;
            }
        }

        public int PC;

        public ushort OpCode { get; private set; }

        public MachineState(Data rom, int origin)
        {
            this.rom = rom;
            memory = new Data(new byte[0xFFFF]);
            PC = origin;

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

        public byte ReadByte(int address)
        {
            return rom.ReadByte(address);
        }

        public ushort ReadWord(int address)
        {
            return rom.ReadWord(address);
        }

        public uint ReadLong(int address)
        {
            return rom.ReadLong(address);
        }

        public byte Read(int address)
        {
            return memory.ReadByte(address);
        }

        public void Write(int address, byte data)
        {
            memory.WriteByte(address, data);
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

        public void WriteAReg(byte register, byte data)
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
    }
}
