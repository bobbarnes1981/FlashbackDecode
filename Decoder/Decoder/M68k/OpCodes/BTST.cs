namespace Decoder.M68k.OpCodes
{
    using System;
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
            : base("0000ddd_00mmmxxx", state)
        {
            this.EffectiveAddress = this.FetchEffectiveAddress();

            if (this.GetBits('_') == 0x1)
            {
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
                this.state.Condition_Z = (this.InterpretEffectiveAddress() & mask) == 0;
            }
            else
            {
                // bit number static
                throw new NotImplementedException();
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
                if (this.GetBits('_') == 0x1)
                {
                    // bit number dynamic
                    return $"{this.Name} {this.GetDn()},{this.GetAssemblyForEffectiveAddress()}";
                }
                else
                {
                    // bit number static
                    throw new NotImplementedException();
                }
            }
        }

        /// <inheritdoc/>
        public override Size Size => this.size;
    }
}
