namespace Decoder.OpCodes
{
    /// <summary>
    /// LEA (Load Effective Address) OpCode.
    /// </summary>
    public class LEA : OpCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LEA"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public LEA(MachineState state)
            : base("0100aaa111mmmxxx", state)
        {
            this.EffectiveAddress = this.FetchEffectiveAddress();

            var srcVal = this.InterpretEffectiveAddress();
            this.WriteValueToEffectiveAddress(EffectiveAddressMode.AddressRegister, (uint)this.GetAn(), srcVal);
        }

        /// <inheritdoc/>
        public override string Name => "LEA";

        /// <inheritdoc/>
        public override string Description => "Load Effective Address";

        /// <inheritdoc/>
        public override string Operation => "<ea> -> An";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} <ea>, An\r\n{this.Name} {this.DescribeEffectiveAddress()}, {this.GetAn()}";

        /// <inheritdoc/>
        public override string Assembly => $"{this.Name} {this.GetAssemblyForEffectiveAddress()}, {this.GetAn()}";

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
