namespace Decoder.M68k.OpCodes
{
    using Decoder.Exceptions;
    using Decoder.M68k.Enums;

    /// <summary>
    /// MOVEA OpCode.
    /// </summary>
    public class MOVEA : OpCode
    {
        private AddressRegister register;

        /// <summary>
        /// Initializes a new instance of the <see cref="MOVEA"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public MOVEA(MegadriveState state)
            : base("00ssaaa001mmmxxx", state)
        {
            this.EffectiveAddress = this.FetchEffectiveAddress();
            this.register = this.GetAn();

            var val = this.InterpretEffectiveAddress();
            this.state.WriteAReg((byte)this.register, val);
        }

        /// <inheritdoc/>
        public override string Name => "MOVEA";

        /// <inheritdoc/>
        public override string Description => "Move Address";

        /// <inheritdoc/>
        public override string Operation => "source -> destination";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} <ea>,An";

        /// <inheritdoc/>
        public override string Assembly => $"{this.Name} {this.GetAssemblyForEffectiveAddress()},{this.GetAn()}";

        /// <inheritdoc/>
        public override Size Size
        {
            get
            {
                switch (this.GetBits('s'))
                {
                    case 0x0002:
                        return Size.Long;
                    case 0x0003:
                        return Size.Word;
                    default:
                        throw new InvalidStateException();
                }
            }
        }
    }
}
