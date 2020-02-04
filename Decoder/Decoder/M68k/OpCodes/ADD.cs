namespace Decoder.M68k.OpCodes
{
    using Decoder.Exceptions;
    using Decoder.M68k.Enums;
    using System;

    /// <summary>
    /// ADD OpCode.
    /// </summary>
    public class ADD : OpCode
    {
        private DataRegister register;
        private MoveDirection direction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ADD"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public ADD(MegadriveState state)
            : base("1101dddDssmmmxxx", state)
        {
            this.EffectiveAddress = this.FetchEffectiveAddress();

            this.register = this.GetDn();
            this.direction = this.GetDirection();

            uint result;
            switch (this.direction)
            {
                // <ea> + Dn -> Dn
                case MoveDirection.RegisterToMemory:
                    {
                        var val = this.ReadValueForEffectiveAddress();
                        var dn = this.state.ReadDReg((byte)this.register);
                        result = val + dn;
                        switch (this.Size)
                        {
                            case Size.Word:
                                this.state.WriteDReg((byte)this.register, (ushort)result);
                                break;
                            case Size.Byte:
                                this.state.WriteDReg((byte)this.register, (byte)result);
                                break;
                            default:
                                throw new InvalidStateException();
                        }
                    }

                    break;

                // Dn + <ea> -> <ea>
                case MoveDirection.MemoryToRegister:
                    {
                        var val = this.ReadValueForEffectiveAddress();
                        var dn = this.state.ReadDReg((byte)this.register);
                        result = val + dn;
                        switch (this.Size)
                        {
                            case Size.Word:
                                this.WriteValueToEffectiveAddress(this.DecodeEffectiveAddressMode(), this.GetXn(), (ushort)result);
                                break;
                            case Size.Byte:
                                this.WriteValueToEffectiveAddress(this.DecodeEffectiveAddressMode(), this.GetXn(), (byte)result);
                                break;
                            default:
                                throw new InvalidStateException();
                        }
                    }

                    break;

                default:
                    throw new InvalidStateException();
            }

            this.state.Condition_X = this.IsCarry(result);
            this.state.Condition_N = this.IsNegative(result);
            this.state.Condition_Z = this.IsZero(result);
            this.state.Condition_V = this.IsOverflow(result);
            this.state.Condition_C = this.IsCarry(result);
        }

        /// <inheritdoc/>
        public override string Name => "ADD";

        /// <inheritdoc/>
        public override string Description => "Add";

        /// <inheritdoc/>
        public override string Operation => "Source + Destination -> Destination";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name}.{this.Size.ToString()[0]} <ea>,Dn\r\n{this.Name} Dn,<ea>";

        /// <inheritdoc/>
        public override string Assembly
        {
            get
            {
                switch (this.direction)
                {
                    case MoveDirection.RegisterToMemory:
                        return $"{this.Name}.{this.Size.ToString()[0]} {this.GetAssemblyForEffectiveAddress()},{this.register}";
                    case MoveDirection.MemoryToRegister:
                        return $"{this.Name}.{this.Size.ToString()[0]} {this.register},{this.GetAssemblyForEffectiveAddress()}";
                    default:
                        throw new InvalidStateException();
                }
            }
        }

        /// <inheritdoc/>
        public override Size Size => (Size)this.GetBits('s');
    }
}
