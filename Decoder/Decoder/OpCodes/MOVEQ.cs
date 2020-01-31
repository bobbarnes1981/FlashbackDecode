using System;

namespace Decoder.OpCodes
{
    /// <summary>
    /// MOVEQ OpCode.
    /// </summary>
    public class MOVEQ : OpCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MOVEQ"/> class.
        /// </summary>
        /// <param name="state">machine state</param>
        public MOVEQ(MachineState state)
            : base("0111ddd0bbbbbbbb", state)
        {
            throw new NotImplementedException();
            // X — Not affected.
            // N — Set if the result is negative; cleared otherwise.
            // Z — Set if the result is zero; cleared otherwise.
            // V — Always cleared.
            // C — Always cleared.
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
        public override string Assembly => $"{this.Name} {this.GetImmediate()},{this.GetDn()}";

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
