namespace Decoder.M68k.OpCodes
{
    using Decoder.M68k.Enums;

    /// <summary>
    /// TST OpCode.
    /// </summary>
    public class TST : OpCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TST"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public TST(MegadriveState state)
            : base("01001010ssmmmxxx", state)
        {
            this.EffectiveAddress = this.FetchEffectiveAddress();

            var val = this.InterpretEffectiveAddress();

            state.Condition_N = this.IsNegative(val);
            state.Condition_Z = this.IsZero(val);
            state.Condition_V = false;
            state.Condition_C = false;
        }

        /// <inheritdoc/>
        public override string Name => "TST";

        /// <inheritdoc/>
        public override string Description => "Test an Operand";

        /// <inheritdoc/>
        public override string Operation => "Destination Tested -> Condition Codes";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} <ea>";

        /// <inheritdoc/>
        public override string Assembly => $"{this.Name}.{this.Size.ToString().ToLower()[0]} {this.GetAssemblyForEffectiveAddress()}";

        /// <inheritdoc/>
        public override Size Size
        {
            get
            {
                return (Size)this.GetBits('s');
            }
        }
    }
}
