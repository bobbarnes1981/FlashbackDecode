namespace Decoder
{
    public class MachineState
    {
        private Data rom;

        private Data memory;

        private uint[] AddressRegisters;

        private uint[] DataRegisters;

        private uint USP;

        private uint SSP;

        public uint SP
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

        public uint PC;

        public ushort OpCode { get; private set; }

        public ushort SR { get; set; }

        public MachineState(Data rom, uint origin)
        {
            // hack to get stop SP wrapping
            USP = 0x9999;

            this.rom = rom;
            memory = new Data(new byte[0x10000]);
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

        public byte ReadByte(uint address)
        {
            return rom.ReadByte(address);
        }

        public ushort ReadWord(uint address)
        {
            return rom.ReadWord(address);
        }

        public uint ReadLong(uint address)
        {
            return rom.ReadLong(address);
        }

        public byte Read(uint address)
        {
            return memory.ReadByte(address);
        }

        public void Write(uint address, byte data)
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
    }
}
