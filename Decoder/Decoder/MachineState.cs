namespace Decoder
{
    class MachineState
    {
        private Data data;

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

        public MachineState(Data data, int origin)
        {
            this.data = data;
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
            OpCode = data.ReadWord(PC);
            PC += 2;
        }

        public byte ReadByte(int address)
        {
            return data.ReadByte(address);
        }

        public ushort ReadWord(int address)
        {
            return data.ReadWord(address);
        }

        public uint ReadLong(int address)
        {
            return data.ReadLong(address);
        }

        public void Write(int address, ushort data)
        {
            System.Console.WriteLine("WARNING: memory write not implemented");
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
