namespace Decoder.OpCodes
{
    public class MOVEM : OpCode
    {
        private ushort mask;

        public override string Name => "MOVEM";

        public override string Description => "Move Multiple Registers";

        public override string Operation => "<list> -> <ea> or <ea> -> <list>";

        public override string Syntax => $"{Name} <list> , <ea>\r\n{Name} <ea>, <list>";

        public override string Assembly
        {
            get
            {
                switch (getDirection())
                {
                    case MoveDirection.MemoryToRegister:
                        return $"{Name} {getEAAssemblyString()}, {mask.ToBinary()}";

                    case MoveDirection.RegisterToMemory:
                        return $"{Name} {mask.ToBinary()}, {getEAAssemblyString()}";

                    default:
                        throw new InvalidStateException();
                }
            }
        }

        protected override string definition => "01001D001smmmxxx";

        public MOVEM(MachineState state)
            : base(state)
        {
            mask = state.ReadWord(state.PC);
            state.PC += 2;

            EA = readEA(decodeEAMode());

            switch (Size)
            {
                //case Size.Word:
                //    switch (getDirection())
                //    {
                //        case MoveDirection.MemoryToRegister:
                //            throw new System.NotImplementedException();
                //        case MoveDirection.RegisterToMemory:
                //            throw new System.NotImplementedException();
                //        default:
                //            throw new InvalidStateException();
                //    }

                //case Size.Long:
                //    switch (getDirection())
                //    {
                //        case MoveDirection.MemoryToRegister:
                //            throw new System.NotImplementedException();
                //        case MoveDirection.RegisterToMemory:
                //            throw new System.NotImplementedException();
                //        default:
                //            throw new InvalidStateException();
                //    }

                //default:
                //    throw new InvalidStateException();
            }
        }

        protected override Size getSize()
        {
            return getSizeFromBits1(6);
        }

        protected MoveDirection getDirection()
        {
            return (MoveDirection)getBits('D');
        }
    }
}
