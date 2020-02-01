namespace Decoder.M68k.OpCodes
{
    using Decoder.M68k.Enums;
    using System;

    /// <summary>
    /// ADXX OpCode.
    /// </summary>
    public class ADDX : OpCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ADDX"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public ADDX(MegadriveState state)
            : base("1101xxx1ss00Dyyy", state)
        {
            throw new NotImplementedException();

            //X — Set the same as the carry bit.
            //N — Set if the result is negative; cleared otherwise.
            //Z — Cleared if the result is nonzero; unchanged otherwise.
            //V — Set if an overflow occurs; cleared otherwise.
            //C — Set if a carry is generated; cleared otherwise. 
        }

        /// <inheritdoc/>
        public override string Name => "ADDX";

        /// <inheritdoc/>
        public override string Description => "Add Extended";

        /// <inheritdoc/>
        public override string Operation => "Source + Destination + X -> Destination";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} Dy,Dx\r\n{this.Name} -(Ay),-(Ax)";

        /// <inheritdoc/>
        public override string Assembly => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public override Size Size => (Size)this.GetBits('s');
    }
}
