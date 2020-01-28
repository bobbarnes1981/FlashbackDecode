using System;

namespace Decoder.OpCodes
{
    public class TST : OpCode
    {
        public override string Name => "TST";

        public override string Description => "Test an Operand";

        public override string Operation => "Destination Tested -> Condition Codes";

        public override string Syntax => $"{Name} <ea>";

        public override string Assembly => $"{Name} {getEAAssemblyString()}";

        protected override string definition => "01001010ssmmmxxx";

        public TST(MachineState state)
            : base(state)
        {
            EA = readEA();

            var val = getEAValue();

            state.Condition_N = isNegative(val);
            state.Condition_Z = isZero(val);
            state.Condition_V = false;
            state.Condition_C = false;
        }

        protected override Size getSize()
        {
            return (Size)getBits('s');
        }
    }
}
