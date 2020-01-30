using Decoder.Exceptions;

namespace Decoder.OpCodes
{
    /// <summary>
    /// Bcc (Branch Condition) OpCode.
    /// </summary>
    public class Bcc : OpCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bcc"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public Bcc(MachineState state)
            : base("0110ccccbbbbbbbb", state)
        {
            if (this.Size == Size.Word)
            {
                this.EffectiveAddress = this.FetchAffectiveAddress(EffectiveAddressMode.Immediate, 0x00);
            }

            if (this.Size == Size.Byte)
            {
                this.EffectiveAddress = (byte)this.GetImmediate();
            }

            if (this.CheckCondition())
            {
                if (this.Size == Size.Word)
                {
                    this.state.PC += this.EffectiveAddress - 2;
                }

                if (this.Size == Size.Byte)
                {
                    this.state.PC += this.EffectiveAddress;
                }
            }
        }

        /// <inheritdoc/>
        public override string Name => "B";

        /// <inheritdoc/>
        public override string Description => "Branch Conditionally";

        /// <inheritdoc/>
        public override string Operation => "If CONDITION TRUE PC+dn -> PC";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name}{this.GetCondition()} <label>";

        /// <inheritdoc/>
        public override string Assembly
        {
            get
            {
                switch (this.Size)
                {
                    case Size.Byte:
                        return $"{this.Name}{this.GetCondition()} ${((sbyte)this.EffectiveAddress) + this.Address:X4}";
                    case Size.Word:
                        return $"{this.Name}{this.GetCondition()} ${((short)this.EffectiveAddress) + this.Address:X4}";
                    default:
                        throw new InvalidStateException();
                }
            }
        }

        /// <inheritdoc/>
        public override Size Size
        {
            get
            {
                return this.GetSizeFrom8BitImmediate();
            }
        }
    }
}
