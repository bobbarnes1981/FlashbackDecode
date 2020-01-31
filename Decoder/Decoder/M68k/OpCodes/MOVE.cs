namespace Decoder.M68k.OpCodes
{
    using Decoder.Exceptions;
    using Decoder.M68k.Enums;

    /// <summary>
    /// MOVE OpCode.
    /// </summary>
    public class MOVE : OpCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MOVE"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public MOVE(MegadriveState state)
            : base("00ss______mmmxxx", state)
        {
            this.SrcEA = this.FetchEffectiveAddress(this.DecodeEffectiveAddressMode(this.GetSrcM(), this.GetSrcXn()), this.GetSrcXn());
            this.DstEA = this.FetchEffectiveAddress(this.DecodeEffectiveAddressMode(this.GetDstM(), this.GetDstXn()), this.GetDstXn());

            var srcVal = this.InterpretEffectiveAddress(this.DecodeEffectiveAddressMode(this.GetSrcM(), this.GetSrcXn()), this.SrcEA, this.GetSrcXn());
            this.WriteValueToEffectiveAddress(this.DecodeEffectiveAddressMode(this.GetDstM(), this.GetDstXn()), this.DstEA, srcVal);

            this.state.Condition_N = this.IsNegative(srcVal);
            this.state.Condition_Z = this.IsZero(srcVal);
            this.state.Condition_V = false;
            this.state.Condition_C = false;
        }

        /// <inheritdoc/>
        public override string Name => "MOVE";

        /// <inheritdoc/>
        public override string Description => "Move Data from Source to Destination";

        /// <inheritdoc/>
        public override string Operation => "<ea> -> <ea>";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} <ea>,<ea>";

        /// <inheritdoc/>
        public override string Assembly
        {
            get
            {
                return $"{this.Name}.{this.Size.ToString().ToLower()[0]} {this.GetAssemblyForEffectiveAddress(this.DecodeEffectiveAddressMode(this.GetSrcM(), this.GetSrcXn()), this.SrcEA, this.GetSrcXn())},{this.GetAssemblyForEffectiveAddress(this.DecodeEffectiveAddressMode(this.GetDstM(), this.GetDstXn()), this.DstEA, this.GetDstXn())}";
            }
        }

        /// <inheritdoc/>
        public override Size Size
        {
            get
            {
                switch (this.GetBits('s'))
                {
                    case 0x0001:
                        return Size.Byte;

                    case 0x0002:
                        return Size.Long;

                    case 0x0003:
                        return Size.Word;

                    default:
                        throw new InvalidStateException();
                }
            }
        }

        /// <summary>
        /// Gets or sets the source effective address.
        /// </summary>
        public uint SrcEA { get; protected set; }

        /// <summary>
        /// Gets or sets the destination effective address.
        /// </summary>
        public uint DstEA { get; protected set; }

        /// <summary>
        /// Get source M.
        /// </summary>
        /// <returns>M value.</returns>
        protected byte GetSrcM()
        {
            return this.GetM();
        }

        /// <summary>
        /// Get destination M.
        /// </summary>
        /// <returns>M value.</returns>
        protected byte GetDstM()
        {
            return (byte)this.state.OpCode.GetBits(6, 3);
        }

        /// <summary>
        /// Get source Xn.
        /// </summary>
        /// <returns>Xn value.</returns>
        protected byte GetSrcXn()
        {
            return this.GetXn();
        }

        /// <summary>
        /// Get destination Xn.
        /// </summary>
        /// <returns>Xn value.</returns>
        protected byte GetDstXn()
        {
            return (byte)this.state.OpCode.GetBits(9, 3);
        }
    }
}
