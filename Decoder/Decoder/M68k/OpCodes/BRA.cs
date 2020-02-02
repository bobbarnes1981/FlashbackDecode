namespace Decoder.M68k.OpCodes
{
    using Decoder.Exceptions;
    using Decoder.M68k.Enums;

    /// <summary>
    /// BRA OpCode.
    /// </summary>
    public class BRA : OpCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BRA"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public BRA(MegadriveState state)
            : base("01100000bbbbbbbb", state)
        {
            if (this.Size == Size.Word)
            {
                this.EffectiveAddress = this.FetchEffectiveAddress(EffectiveAddressMode.Immediate, 0x00);
            }
            else
            {
                this.EffectiveAddress = this.GetImmediate();
            }

            if (this.Size == Size.Word)
            {
                this.state.PC += this.EffectiveAddress - 2;
            }
            else
            {
                this.state.PC += this.EffectiveAddress;
            }
        }

        /// <inheritdoc/>
        public override string Name => "BRA";

        /// <inheritdoc/>
        public override string Description => "Branch Always";

        /// <inheritdoc/>
        public override string Operation => "PC + dn -> PC";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} <label>";

        /// <inheritdoc/>
        public override string Assembly
        {
            get
            {
                switch (this.Size)
                {
                    case Size.Byte:
                        return $"{this.Name} {(sbyte)this.EffectiveAddress}";
                    case Size.Word:
                        return $"{this.Name} {(short)this.EffectiveAddress}";
                    default:
                        throw new InvalidStateException();
                }
            }
        }

        /// <inheritdoc/>
        public override Size Size => this.GetSizeFrom8BitImmediate();
    }
}
