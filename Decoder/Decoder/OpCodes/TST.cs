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
            EffectiveAddress = readEA();

            var val = getEAValue();

            state.Condition_N = IsNegative(val);
            state.Condition_Z = IsZero(val);
            state.Condition_V = false;
            state.Condition_C = false;
        }

        protected override Size getSize()
        {
            return (Size)GetBits('s');
        }
    }
}
