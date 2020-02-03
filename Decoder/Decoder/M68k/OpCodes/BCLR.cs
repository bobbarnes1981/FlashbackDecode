namespace Decoder.M68k.OpCodes
{
    using System;
    using Decoder.Exceptions;
    using Decoder.M68k.Enums;

    /// <summary>
    /// BCLR OpCode.
    /// </summary>
    public class BCLR : OpCode
    {
        private readonly Size size;

        /// <summary>
        /// Initializes a new instance of the <see cref="BCLR"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public BCLR(MegadriveState state)
            : base("0000ddd___mmmxxx", state)
        {
            this.EffectiveAddress = this.FetchEffectiveAddress();

            switch (this.GetBits('_'))
            {
                case 0x6:

                    // bit number dynamic
                    var dn = this.GetDn();
                    var bitnumber = this.state.ReadDReg((byte)dn);
                    uint mask = 0x00000001;
                    if (this.DecodeEffectiveAddressMode() == EffectiveAddressMode.DataRegister)
                    {
                        this.size = Size.Long;
                        bitnumber = bitnumber % 32;
                    }
                    else
                    {
                        this.size = Size.Byte;
                        bitnumber = bitnumber % 8;
                    }

                    mask = (uint)(mask << (int)bitnumber);
                    var val = this.ReadValueForEffectiveAddress();
                    this.state.Condition_Z = (val & mask) == 0;

                    this.WriteValueToEffectiveAddress(this.DecodeEffectiveAddressMode(), this.GetXn(), val & ~mask);

                    break;

                case 0x2:

                    // bit number static
                    throw new NotImplementedException();

                default:
                    throw new InvalidStateException();
            }
        }

        /// <inheritdoc/>
        public override string Name => "BCLR";

        /// <inheritdoc/>
        public override string Description => "Test a Bit and Clear";

        /// <inheritdoc/>
        public override string Operation => "TEST (<bitnumber> of Destination) -> Z; 0 -> <bitnumber> of Destination";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} Dn,<ea>\r\n{this.Name} #<data>,<ea>";

        /// <inheritdoc/>
        public override string Assembly
        {
            get
            {
                if (this.GetBits('_') == 0x6)
                {
                    // bit number dynamic
                    return $"{this.Name} {this.GetDn()},{this.GetAssemblyForEffectiveAddress()}";
                }

                if (this.GetBits('_') == 0x2)
                {
                    // bit number static
                    throw new NotImplementedException();
                }

                throw new InvalidStateException();
            }
        }

        /// <inheritdoc/>
        public override Size Size => this.size;
    }
}
