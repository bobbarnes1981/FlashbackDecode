namespace Decoder.OpCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
        public ANDI(MachineState state)
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

            var val = this.InterpretEffectiveAddress();
            val &= this.immediate;
            this.WriteValueToEffectiveAddress(this.DecodeEffectiveAddressMode(), this.GetXn(), val);

            // X — Not affected.
            // N — Set if the most significant bit of the result is set; cleared otherwise.
            // Z — Set if the result is zero; cleared otherwise.
            // V — Always cleared.
            // C — Always cleared.
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
        public override Size Size
        {
            get
            {
                return (Size)this.GetBits('s');
            }
        }
    }
}
