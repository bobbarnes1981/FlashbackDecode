namespace Decoder.M68k.OpCodes
{
    using Decoder.M68k.Enums;

    /// <summary>
    /// MOVEQ OpCode.
    /// </summary>
    public class MOVEQ : OpCode
    {
        private uint immediate;

        private DataRegister register;

        /// <summary>
        /// Initializes a new instance of the <see cref="MOVEQ"/> class.
        /// </summary>
        /// <param name="state">machine state</param>
        public MOVEQ(MegadriveState state)
            : base("0111ddd0bbbbbbbb", state)
        {
            this.immediate = this.GetImmediate();
            this.register = this.GetDn();

            this.state.WriteDReg((byte)this.register, this.immediate);

            this.state.Condition_N = this.IsNegative(this.immediate);
            this.state.Condition_Z = this.IsZero(this.immediate);
            this.state.Condition_V = false;
            this.state.Condition_C = false;
        }

        /// <inheritdoc/>
        public override string Name => "MOVEQ";

        /// <inheritdoc/>
        public override string Description => "Move Quick";

        /// <inheritdoc/>
        public override string Operation => "Immediate data -> Destination";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} #<data>, Dn";

        /// <inheritdoc/>
        public override string Assembly => $"{this.Name} #{this.GetImmediate()},{this.GetDn()}";

        /// <inheritdoc/>
        public override Size Size
        {
            get
            {
                return Size.Long;
            }
        }
    }
}
