using System;

namespace Decoder.OpCodes
{
    class SWAP : OpCode
    {
        public override string Name => throw new NotImplementedException();

        public override string Description => throw new NotImplementedException();

        public override string Operation => throw new NotImplementedException();

        public override string Syntax => throw new NotImplementedException();

        public override string Assembly => throw new NotImplementedException();

        protected override string definition => throw new NotImplementedException();

        public SWAP(MachineState state)
            : base(state)
        {
        }

        protected override Size getSize()
        {
            throw new NotImplementedException();
        }
    }
}
