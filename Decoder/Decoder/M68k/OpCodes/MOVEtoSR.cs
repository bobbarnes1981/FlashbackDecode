namespace Decoder.M68k.OpCodes
{
    using Decoder.M68k.Enums;

    /// <summary>
    /// MOVE to SR OpCode.
    /// </summary>
    public class MOVEtoSR : OpCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MOVEtoSR"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public MOVEtoSR(MegadriveState state)
            : base("0100011011mmmxxx", state)
        {
            this.EffectiveAddress = this.FetchEffectiveAddress();
            this.state.SR = (ushort)this.ReadValueForEffectiveAddress();
        }

        /// <inheritdoc/>
        public override string Name => "MOVE";

        /// <inheritdoc/>
        public override string Description => "Move to Status Register";

        /// <inheritdoc/>
        public override string Operation => "<ea> -> SR";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} <ea>, SR";

        /// <inheritdoc/>
        public override string Assembly => $"{this.Name} {this.GetAssemblyForEffectiveAddress()}, SR";

        /// <inheritdoc/>
        public override Size Size => Size.Word;
    }
}
