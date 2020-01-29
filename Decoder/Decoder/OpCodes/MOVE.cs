namespace Decoder.OpCodes
{
    public class MOVE : OpCode
    {
        public override string Name => "MOVE";

        public override string Description => "Move Data from Source to Destination";

        public override string Operation => "<ea> -> <ea>";

        public override string Syntax => $"{Name} <ea>, <ea>";

        public override string Assembly
        {
            get
            {
                return $"{Name} {getEAAssemblyString(DecodeEffectiveAddressMode(GetSrcM(), GetSrcXn()), SrcEA, GetSrcXn())}, {getEAAssemblyString(DecodeEffectiveAddressMode(GetDstM(), GetDstXn()), DstEA, GetDstXn())}";
            }
        }

        public uint SrcEA { get; protected set; }
        public uint DstEA { get; protected set; }

        public MOVE(MachineState state)
            : base("00ss______mmmxxx", state)
        {
            this.SrcEA = this.readEA(this.DecodeEffectiveAddressMode(this.GetSrcM(), this.GetSrcXn()), this.GetSrcXn());
            this.DstEA = this.readEA(this.DecodeEffectiveAddressMode(this.GetDstM(), this.GetDstXn()), this.GetDstXn());

            var srcVal = getEAValue(this.DecodeEffectiveAddressMode(this.GetSrcM(), this.GetSrcXn()), this.EffectiveAddress, (byte)this.SrcEA);
            this.setEAValue(this.DecodeEffectiveAddressMode(this.GetDstM(), this.GetDstXn()), this.DstEA, srcVal);
        }

        protected byte GetSrcM()
        {
            return GetM();
        }

        protected byte GetDstM()
        {
            return (byte)this.state.OpCode.GetBits(6, 3);
        }

        protected override Size getSize()
        {
            switch (this.GetBits('s'))
            {
                case 0x0001:
                    return Size.Byte;

                case 0x0002:
                    return Size.Long;

                case 0x0003:
                    return Size.Word;

                default:
                    throw new InvalidStateException();
            }
        }

        protected byte GetSrcXn()
        {
            return this.GetXn();
        }

        protected byte GetDstXn()
        {
            return (byte)this.state.OpCode.GetBits(9, 3);
        }
    }
}
