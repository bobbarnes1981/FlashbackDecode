namespace Decoder.OpCodes
{
    public class LEA : OpCode
    {
        public override string Name => "LEA";

        public override string Description => "Load Effective Address";

        public override string Operation => "<ea> -> An";

        public override string Syntax => $"{Name} <ea>, An\r\n{Name} {getEADescriptionString()}, {GetAn()}";

        public override string Assembly => $"{Name} {getEAAssemblyString()}, {GetAn()}";

        protected override string definition => "0100aaa111mmmxxx";

        public LEA(MachineState state)
            : base(state)
        {
            EffectiveAddress = readEA(DecodeEffectiveAddressMode());

            var srcVal = getEAValue();
            setEAValue(EffectiveAddressMode.AddressRegister, (uint)GetAn(), srcVal);
        }

        protected override Size getSize()
        {
            return Size.Long;
        }
    }
}
