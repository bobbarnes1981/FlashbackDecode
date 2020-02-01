namespace Decoder.M68k.OpCodes
{
    using Decoder.Exceptions;
    using Decoder.M68k.Enums;

    /// <summary>
    /// MOVEM OpCode.
    /// </summary>
    public class MOVEM : OpCode
    {
        /// <summary>
        /// Register mask.
        /// </summary>
        private readonly ushort mask;

        /// <summary>
        /// Initializes a new instance of the <see cref="MOVEM"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public MOVEM(MegadriveState state)
            : base("01001D001smmmxxx", state)
        {
            this.mask = state.ReadWord(state.PC);
            state.PC += 2;

            this.EffectiveAddress = this.FetchEffectiveAddress();

            // TODO: should the increment only include values that are beingset?
            switch (this.Size)
            {
                case Size.Word:
                    switch (this.GetDirection())
                    {
                        case MoveDirection.MemoryToRegister:
                            ushort maskCheck = 1;
                            int count = 0;
                            for (int i = 0; i < 8; i++)
                            {
                                uint d = state.ReadWord((uint)(this.EffectiveAddress + (count * 2)));
                                if ((this.mask & maskCheck) == maskCheck)
                                {
                                    state.WriteDReg((byte)i, d);
                                    count++;
                                }

                                maskCheck += maskCheck;
                            }

                            for (int i = 0; i < 8; i++)
                            {
                                uint d = state.ReadWord((uint)(this.EffectiveAddress + (count * 2)));
                                if ((this.mask & maskCheck) == maskCheck)
                                {
                                    state.WriteAReg((byte)i, d);
                                    count++;
                                }

                                maskCheck += maskCheck;
                            }

                            this.WriteValueToEffectiveAddress(this.DecodeEffectiveAddressMode(), this.GetXn(), (uint)(this.EffectiveAddress + (count * 2)));

                            break;
                        case MoveDirection.RegisterToMemory:
                            throw new System.NotImplementedException();
                        default:
                            throw new InvalidStateException();
                    }

                    break;

                case Size.Long:
                    switch (this.GetDirection())
                    {
                        case MoveDirection.MemoryToRegister:
                            ushort maskCheck = 1;
                            int count = 0;
                            for (int i = 0; i < 8; i++)
                            {
                                uint d = state.ReadLong((uint)(this.EffectiveAddress + (count * 4)));
                                if ((this.mask & maskCheck) == maskCheck)
                                {
                                    state.WriteDReg((byte)i, d);
                                    count++;
                                }

                                maskCheck += maskCheck;
                            }

                            for (int i = 0; i < 8; i++)
                            {
                                uint d = state.ReadLong((uint)(this.EffectiveAddress + (count * 4)));
                                if ((this.mask & maskCheck) == maskCheck)
                                {
                                    state.WriteAReg((byte)i, d);
                                    count++;
                                }

                                maskCheck += maskCheck;
                            }

                            this.WriteValueToEffectiveAddress(this.DecodeEffectiveAddressMode(), this.GetXn(), (uint)(this.EffectiveAddress + (count * 4)));

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

        /// <inheritdoc/>
        public override string Name => "MOVEM";

        /// <inheritdoc/>
        public override string Description => "Move Multiple Registers";

        /// <inheritdoc/>
        public override string Operation => "<list> -> <ea> or <ea> -> <list>";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} <list>,<ea>\r\n{this.Name} <ea>,<list>";

        /// <inheritdoc/>
        public override string Assembly
        {
            get
            {
                switch (this.GetDirection())
                {
                    case MoveDirection.MemoryToRegister:
                        return $"{this.Name} {this.GetAssemblyForEffectiveAddress()},%{this.mask.ToBinary()}";

                    case MoveDirection.RegisterToMemory:
                        return $"{this.Name} %{this.mask.ToBinary()},{this.GetAssemblyForEffectiveAddress()}";

                    default:
                        throw new InvalidStateException();
                }
            }
        }

        /// <inheritdoc/>
        public override Size Size => this.GetSizeFrom1BitImmediate(6);

        /// <summary>
        /// MOVEM will write to the register instead of the referenced address.
        /// </summary>
        /// <param name="ea">Effective Address Mode.</param>
        /// <param name="Xn">Xn value.</param>
        /// <param name="value">Value to write.</param>
        protected void WriteValueToEffectiveAddress(EffectiveAddressMode ea, uint Xn, uint value)
        {
            switch (ea)
            {
                case EffectiveAddressMode.AddressPostIncrement:
                    switch (this.Size)
                    {
                        case Size.Byte:
                            this.state.WriteAReg((byte)Xn, value);
                            break;
                        case Size.Word:
                            this.state.WriteAReg((byte)Xn, value);
                            break;
                        case Size.Long:
                            this.state.WriteAReg((byte)Xn, value);
                            break;
                        default:
                            throw new InvalidStateException();
                    }

                    break;

                case EffectiveAddressMode.Address:
                    switch (this.Size)
                    {
                        case Size.Byte:
                            this.state.WriteAReg((byte)Xn, value);
                            break;
                        case Size.Word:
                            this.state.WriteAReg((byte)Xn, value);
                            break;
                        case Size.Long:
                            this.state.WriteAReg((byte)Xn, value);
                            break;
                        default:
                            throw new InvalidStateException();
                    }

                    break;

                default:
                    throw new InvalidStateException();
            }
        }
    }
}
