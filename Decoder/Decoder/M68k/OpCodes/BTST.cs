namespace Decoder.M68k.OpCodes
{
    using System;
    using Decoder.Exceptions;
    using Decoder.M68k.Enums;

    /// <summary>
    /// BTST OpCode.
    /// </summary>
    public class BTST : OpCode
    {
        private readonly Size size;

        /// <summary>
        /// Initializes a new instance of the <see cref="BTST"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public BTST(MegadriveState state)
            : base("0000ddd___mmmxxx", state)
        {
            this.EffectiveAddress = this.FetchEffectiveAddress();

            switch (this.GetBits('_'))
            {
                case 0x4:

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
                    this.state.Condition_Z = (this.ReadValueForEffectiveAddress() & mask) == 0;

                    break;

                case 0x0:

                    // bit number static
                    throw new NotImplementedException();

                default:
                    throw new InvalidStateException();
            }
        }

        /// <inheritdoc/>
        public override string Name => "BTST";

        /// <inheritdoc/>
        public override string Description => "Test a Bit";

        /// <inheritdoc/>
        public override string Operation => "TEST (<bitnumber> of destination) -> Z";

        /// <inheritdoc/>
        public override string Syntax => "BTST Dn,<ea>\r\nBTST #<data>,<Ea>";

        /// <inheritdoc/>
        public override string Assembly
        {
            get
            {
                switch (this.GetBits('_'))
                {
                    case 0x4:

                        // bit number dynamic
                        return $"{this.Name} {this.GetDn()},{this.GetAssemblyForEffectiveAddress()}";

                    case 0x0:

                        // bit number static
                        throw new NotImplementedException();

                    default:
                        throw new InvalidStateException();
                }
            }
        }

        /// <inheritdoc/>
        public override Size Size => this.size;
    }
}
