namespace Decoder.M68k.OpCodes
{
    using Decoder.Exceptions;
    using Decoder.M68k.Enums;
    using System;

    /// <summary>
    /// MOVE USP OpCode.
    /// </summary>
    public class MOVEUSP : OpCode
    {
        private MoveDirection direction;
        private AddressRegister register;

        /// <summary>
        /// Initializes a new instance of the <see cref="MOVEUSP"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public MOVEUSP(MegadriveState state)
            : base("010011100110Daaa", state)
        {
            this.direction = this.GetDirection();
            this.register = this.GetAn();

            if (this.state.Condition_Supervisor)
            {
                switch (this.direction)
                {
                    case MoveDirection.MemoryToRegister:
                        throw new NotImplementedException();
                        break;
                    case MoveDirection.RegisterToMemory:
                        throw new NotImplementedException();
                        break;
                    default:
                        throw new InvalidStateException();
                }
            }
            else
            {
                Writer.Write("TRAP", ConsoleColor.Red);
            }
        }

        /// <inheritdoc/>
        public override string Name => "MOVE";

        /// <inheritdoc/>
        public override string Description => "Move User Stack Pointer (Privileged Instruction)";

        /// <inheritdoc/>
        public override string Operation => "If Supervisor State Then USP -> An or An -> USP Else TRAP";

        /// <inheritdoc/>
        public override string Syntax => $"{this.Name} USP,An\r\n{this.Name} An,USP";

        /// <inheritdoc/>
        public override string Assembly
        {
            get
            {
                switch (this.GetDirection())
                {
                    case MoveDirection.MemoryToRegister:
                        return $"{this.Name} USP,{this.register}";

                    case MoveDirection.RegisterToMemory:
                        return $"{this.Name} {this.register},USP";

                    default:
                        throw new InvalidStateException();
                }
            }
        }

        /// <inheritdoc/>
        public override Size Size => Size.Long;

        /// <summary>
        /// Get direction.
        /// </summary>
        /// <returns>direction.</returns>
        protected MoveDirection GetDirection()
        {
            return (MoveDirection)this.GetBits('D');
        }
    }
}
