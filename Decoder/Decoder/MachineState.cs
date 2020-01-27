namespace Decoder
{
    class MachineState
    {
        private Data data;

        public int PC;

        public ushort OpCode { get; private set; }

        public MachineState(Data data, int origin)
        {
            this.data = data;
            PC = origin;
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
    }
}
