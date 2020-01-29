namespace Decoder.OpCodes
{
    public class MOVEM : OpCode
    {
        private ushort mask;

        public override string Name => "MOVEM";

        public override string Description => "Move Multiple Registers";

        public override string Operation => "<list> -> <ea> or <ea> -> <list>";

        public override string Syntax => $"{Name} <list> ,<ea>\r\n{Name} <ea>, <list>";

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

        public MOVEM(MachineState state)
            : base("01001D001smmmxxx", state)
        {
            this.mask = state.ReadWord(state.PC);
            state.PC += 2;

            this.EffectiveAddress = this.readEA(this.DecodeEffectiveAddressMode());

            // TODO: should the increment only include values that are beingset?
            switch (this.Size)
            {
                case Size.Word:
                    switch (this.getDirection())
                    {
                        case MoveDirection.MemoryToRegister:
                            ushort maskCheck = 1;
                            for (int i = 0; i < 8; i++)
                            {
                                uint d = state.ReadWord((uint)(this.EffectiveAddress + (i * 2)));
                                if ((this.mask & maskCheck) == maskCheck)
                                {
                                    state.WriteDReg((byte)i, d);
                                }

                                maskCheck += maskCheck;
                            }

                            for (int i = 0; i < 8; i++)
                            {
                                uint d = state.ReadWord((uint)(this.EffectiveAddress + (i * 2) + 16));
                                if ((this.mask & maskCheck) == maskCheck)
                                {
                                    state.WriteAReg((byte)i, d);
                                }

                                maskCheck += maskCheck;
                            }

                            this.setEAValue(this.DecodeEffectiveAddressMode(), this.EffectiveAddress + (16 * 2));

                            break;
                        case MoveDirection.RegisterToMemory:
                            throw new System.NotImplementedException();
                        default:
                            throw new InvalidStateException();
                    }

                    break;

                case Size.Long:
                    switch (this.getDirection())
                    {
                        case MoveDirection.MemoryToRegister:
                            ushort maskCheck = 1;
                            for (int i = 0; i < 8; i++)
                            {
                                uint d = state.ReadLong((uint)(this.EffectiveAddress + (i * 4)));
                                if ((this.mask & maskCheck) == maskCheck)
                                {
                                    state.WriteDReg((byte)i, d);
                                }

                                maskCheck += maskCheck;
                            }

                            for (int i = 0; i < 8; i++)
                            {
                                uint d = state.ReadLong((uint)(this.EffectiveAddress + (i * 4) + 32));
                                if ((this.mask & maskCheck) == maskCheck)
                                {
                                    state.WriteAReg((byte)i, d);
                                }

                                maskCheck += maskCheck;
                            }

                            this.setEAValue(this.DecodeEffectiveAddressMode(), this.EffectiveAddress + (32 * 2));

                            break;
                        case MoveDirection.RegisterToMemory:
                            throw new System.NotImplementedException();
                        default:
                            throw new InvalidStateException();
                    }

                    break;

                default:
                    throw new InvalidStateException();
            }
        }

        protected override Size getSize()
        {
            return this.getSizeFrom1Bit(6);
        }

        protected MoveDirection getDirection()
        {
            return (MoveDirection)this.GetBits('D');
        }
    }
}
