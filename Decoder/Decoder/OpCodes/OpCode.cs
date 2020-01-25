namespace Decoder.OpCodes
{
    abstract class OpCode
    {
        protected readonly Data data;
        protected readonly int address;
        protected readonly ushort code;

        public readonly string Name;

        public readonly string Description;

        public readonly Size Size;

        // TODO: int?
        public int EA { get; protected set; }

        public int PCDisplacement { get; protected set; }

        public string FullName
        {
            get
            {
                return string.Format("{0}.{1}", Name, Size.ToString().ToLower()[0]);
            }
        }

        public OpCode(Data data, int address, ushort code, string name, string description)
        {
            this.data = data;
            this.address = address;
            this.code = code;

            this.Name = name;
            this.Description = description;

            this.Size = getSize();

            PCDisplacement = 2;
        }

        protected abstract Size getSize();

        protected abstract AddressRegister getAn();

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
                    return string.Format("0x{0:X4} (d16, A{1})", ea, xn);

                case EffectiveAddressMode.Address:
                    return string.Format("0x{0:X4} (A{1})", ea, xn);

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

        public abstract string Operation();
    }
}
