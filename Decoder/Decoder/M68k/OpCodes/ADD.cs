namespace Decoder.M68k.OpCodes
{
    using Decoder.M68k.Enums;
    using System;

    /// <summary>
    /// ADD OpCode.
    /// </summary>
    public class ADD : OpCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ADD"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public ADD(MegadriveState state)
            : base("1101dddDssmmmxxx", state)
        {
            throw new NotImplementedException();

            //X — Set the same as the carry bit.
            //N — Set if the result is negative; cleared otherwise.
            //Z — Set if the result is zero; cleared otherwise.
            //V — Set if an overflow is generated; cleared otherwise.
            //C — Set if a carry is generated; cleared otherwise. 
        }

        /// <inheritdoc/>
        public override string Name => "ADD";

        /// <inheritdoc/>
        public override string Description => "Add";

        /// <inheritdoc/>
        public override string Operation => "Source + Destination -> Destination";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} <ea>,Dn\r\n{this.Name} Dn,<ea>";

        /// <inheritdoc/>
        public override string Assembly => throw new NotImplementedException();

        /// <inheritdoc/>
        public override Size Size => throw new NotImplementedException();
    }
}
