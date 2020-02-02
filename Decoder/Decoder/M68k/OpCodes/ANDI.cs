namespace Decoder.M68k.OpCodes
{
    using Decoder.M68k.Enums;

    /// <summary>
    /// ANDI OpCode.
    /// </summary>
    public class ANDI : OpCode
    {
        private uint immediate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ANDI"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public ANDI(MegadriveState state)
            : base("00000010ssmmmxxx", state)
        {
            var allowedModes = new[]
            {
                EffectiveAddressMode.DataRegister,
                EffectiveAddressMode.Address,
                EffectiveAddressMode.AddressPostIncrement,
                EffectiveAddressMode.AddressPreDecrement,
                EffectiveAddressMode.AddressWithDisplacement,
                EffectiveAddressMode.AddressWithIndex,
                EffectiveAddressMode.AbsoluteWord,
                EffectiveAddressMode.AbsoluteLong,
            };
            this.ValidateEffectiveAddress(this.DecodeEffectiveAddressMode(), allowedModes);

            this.EffectiveAddress = this.FetchEffectiveAddress();
            this.immediate = this.ReadImmediate();

            var val = this.ReadValueForEffectiveAddress();
            val &= this.immediate;
            this.WriteValueToEffectiveAddress(this.DecodeEffectiveAddressMode(), this.GetXn(), val);

            this.state.Condition_N = this.IsNegative(val);
            this.state.Condition_Z = this.IsZero(val);
            this.state.Condition_V = false;
            this.state.Condition_C = false;
        }

        /// <inheritdoc/>
        public override string Name => "ANDI";

        /// <inheritdoc/>
        public override string Description => "AND Immediate";

        /// <inheritdoc/>
        public override string Operation => "Immediate Data ^ Destination -> Destination";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} #<data>,<ea>";

        /// <inheritdoc/>
        public override string Assembly => $"{this.Name} #{this.immediate},{this.GetAssemblyForEffectiveAddress()}";

        /// <inheritdoc/>
        public override Size Size => (Size)this.GetBits('s');
    }
}
