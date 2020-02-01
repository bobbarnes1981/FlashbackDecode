namespace Decoder.M68k.OpCodes
{
    using Decoder.M68k.Enums;
    using System;

    /// <summary>
    /// DBcc OpCode.
    /// </summary>
    public class DBcc : OpCode
    {
        private short displacement;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBcc"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public DBcc(MegadriveState state)
            : base("0101cccc11001ddd", state)
        {
            this.displacement = (short)this.ReadDataUsingPC(this.Size); // always WORD

            if (!this.CheckCondition())
            {
                short dn = (short)(this.state.ReadDReg((byte)this.GetDn()) - 1);
                this.state.WriteDReg((byte)this.GetDn(), (uint)dn);
                if (dn != -1)
                {
                    this.state.PC = (uint)(this.state.PC + this.displacement);
                }
            }
        }

        /// <inheritdoc/>
        public override string Name => "DB";

        /// <inheritdoc/>
        public override string Description => "Test Condition, Decrement and Branch";

        /// <inheritdoc/>
        public override string Operation => "IF CONDITON FALSE (Dn–1 -> Dn; If Dn != – 1 Then PC + dn -> PC)";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} Dn, <label>";

        /// <inheritdoc/>
        public override string Assembly => $"{this.Name}.{this.GetCondition()} {this.GetDn()}, ${this.displacement:X4}";

        /// <inheritdoc/>
        public override Size Size => Size.Word;
    }
}
