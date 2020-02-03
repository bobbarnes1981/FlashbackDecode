namespace Decoder.M68k.OpCodes
{
    using Decoder.M68k.Enums;

    /// <summary>
    /// ADDQ OpCode.
    /// </summary>
    public class ADDQ : OpCode
    {
        private ushort immediate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ADDQ"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public ADDQ(MegadriveState state)
            : base("0101bbb0ssmmmxxx", state)
        {
            this.EffectiveAddress = this.FetchEffectiveAddress();
            this.immediate = this.GetImmediate();

            var val = this.ReadValueForEffectiveAddress();
            uint result = val + this.immediate;
            this.WriteValueToEffectiveAddress(this.DecodeEffectiveAddressMode(), this.GetXn(), result);

            if (this.DecodeEffectiveAddressMode() != EffectiveAddressMode.AddressRegister)
            {
                this.state.Condition_X = this.IsCarry(result);
                this.state.Condition_N = this.IsNegative(result);
                this.state.Condition_Z = this.IsZero(result);
                this.state.Condition_V = this.IsOverflow(result);
                this.state.Condition_C = this.IsCarry(result);
            }
        }

        /// <inheritdoc/>
        public override string Name => "ADDQ";

        /// <inheritdoc/>
        public override string Description => "Add Quick";

        /// <inheritdoc/>
        public override string Operation => "Immediate Data + Destination -> Destination";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} #<data>, <ea>";

        /// <inheritdoc/>
        public override string Assembly => $"{this.Name} #{this.immediate}, {this.GetAssemblyForEffectiveAddress()}";

        /// <inheritdoc/>
        public override Size Size => (Size)this.GetBits('s');
    }
}
