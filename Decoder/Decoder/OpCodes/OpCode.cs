namespace Decoder.OpCodes
{
    abstract class OpCode
    {
        protected readonly Data data;
        protected readonly int address;
        protected readonly ushort code;

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string Operation { get; }

        public abstract string Syntax { get; }

        public abstract string Assembly { get; }

        public readonly Size Size;

        // TODO: int?
        public int EA { get; protected set; }

        // TODO: add operation length so we can skip addresses in disassembly

        public int PCDisplacement { get; protected set; }

        public string FullName
        {
            get
            {
                return string.Format("{0}.{1}", Name, Size.ToString().ToLower()[0]);
            }
        }

        public OpCode(Data data, int address, ushort code)
        {
            this.data = data;
            this.address = address;
            this.code = code;

            this.Size = getSize();

            PCDisplacement = 2;
        }

        protected abstract Size getSize();

        protected abstract AddressRegister getAn();

        protected abstract DataRegister getDn();

        protected abstract byte getM();

        protected abstract byte getXn();

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
                    return string.Format("(D{0})", xn);

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
            // TODO: return value? register? ??!?!
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

                case EffectiveAddressMode.Address_PostIncremenet:
                    return readAReg(Xn);

                default:
                    throw new System.NotImplementedException(ea.ToString());
            }
        }

        private ushort readAReg(byte register)
        {
            System.Console.WriteLine("WARNING: register An reading not implemented");
            return 0x00;
        }

        protected int readData(Size size)
        {
            switch (size)
            {
                case Size.Long:
                    uint l = data.ReadLong(address + PCDisplacement);
                    PCDisplacement += 4;
                    return (int)l;

                case Size.Word:
                    ushort w = data.ReadWord(address + PCDisplacement);
                    PCDisplacement += 2;
                    return (int)w;

                default:
                    throw new System.Exception();
            }
        }
    }
}
