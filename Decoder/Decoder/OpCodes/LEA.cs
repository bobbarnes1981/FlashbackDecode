namespace Decoder.OpCodes
{
    public class LEA : OpCode
    {
        public override string Name => "LEA";

        public override string Description => "Load Effective Address";

        public override string Operation => "<ea> -> An";

        public override string Syntax => $"{Name} <ea>, An\r\n{Name} {getEADescriptionString()}, {getAn()}";

        public override string Assembly => $"{Name} {getEAAssemblyString()}, {getAn()}";

        protected override string definition => "0100aaa111mmmxxx";

        public LEA(MachineState state)
            : base(state)
        {
            EA = readEA(decodeEAMode());

            var srcVal = getEAValue();
            setEAValue(EffectiveAddressMode.AddressRegister, (uint)getAn(), srcVal);
        }

        protected override Size getSize()
        {
            return Size.Long;
        }
    }
}
