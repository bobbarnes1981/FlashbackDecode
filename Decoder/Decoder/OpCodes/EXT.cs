using System;

namespace Decoder.OpCodes
{
    class EXT : OpCode
    {
        protected override string definition => "010010001s000ddd";

        public override string Name => "EXT";

        public override string Description => "Sign Extend";

        public override string Operation => throw new NotImplementedException();

        public override string Syntax => throw new NotImplementedException();

        public override string Assembly => throw new NotImplementedException();

        public EXT(Data data, int address, ushort code)
            : base(data, address, code)
        {
        }

        protected override Size getSize()
        {
            throw new NotImplementedException();
        }
    }
}
