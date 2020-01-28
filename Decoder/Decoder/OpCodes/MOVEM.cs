namespace Decoder.OpCodes
{
    public class MOVEM : OpCode
    {
        protected override string definition => "01001D001smmmxxx";

        private ushort mask;

        public override string Name => "MOVEM";

        public override string Description => "Move Multiple Registers";

        public override string Operation => "<list> -> <ea> or <ea> -> <list>";

        public override string Syntax => string.Format("{0} <list> , <ea>\r\n{0} <ea>, <list>", Name);

        public override string Assembly
        {
            get
            {
                switch (getDirection())
                {
                    case MoveDirection.MemoryToRegister:
                        return string.Format("{0} {1}, {2}", Name, getEAString(decodeEAMode(), EA), mask.ToBinary());

                    case MoveDirection.RegisterToMemory:
                        return string.Format("{0} {2}, {1}", Name, getEAString(decodeEAMode(), EA), mask.ToBinary());

                    default:
                        throw new InvalidStateException();
                }
            }
        }

        public MOVEM(MachineState state)
            : base(state)
        {
            mask = state.ReadWord(state.PC);
            state.PC += 2;

            EA = readEA(decodeEAMode());

            //switch (Size)
            //{
            //    case Size.Word:
            //        switch (getDirection())
            //        {
            //            case MoveDirection.MemoryToRegister:
            //                throw new System.NotImplementedException();
            //            case MoveDirection.RegisterToMemory:
            //                throw new System.NotImplementedException();
            //            default:
            //                throw new InvalidStateException();
            //        }

            //    case Size.Long:
            //        switch (getDirection())
            //        {
            //            case MoveDirection.MemoryToRegister:
            //                throw new System.NotImplementedException();
            //            case MoveDirection.RegisterToMemory:
            //                throw new System.NotImplementedException();
            //            default:
            //                throw new InvalidStateException();
            //        }

            //    default:
            //        throw new InvalidStateException();
            //}
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
