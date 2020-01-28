using System;

namespace Decoder.OpCodes
{
    class Invalid : OpCode
    {
        protected override string definition => throw new NotImplementedException();

        public override string Name => "INVALID";

        public override string Description => throw new NotImplementedException();

        public override string Operation => throw new NotImplementedException();

        public override string Syntax => throw new NotImplementedException();

        public override string Assembly => throw new NotImplementedException();

        public Invalid(MachineState state)
            : base(state)
        {
        }

        protected override Size getSize()
        {
            throw new NotImplementedException();
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
