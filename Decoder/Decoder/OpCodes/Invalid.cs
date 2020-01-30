namespace Decoder.OpCodes
{
    using System;

    /// <summary>
    /// Invalid OpCode.
    /// </summary>
    public class Invalid : OpCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Invalid"/> class.
        /// </summary>
        /// <param name="state">machine state.</param>
        public Invalid(MachineState state)
            : base("----------------", state)
        {
        }

        /// <inheritdoc/>
        public override string Name => "INVALID";

        /// <inheritdoc/>
        public override string Description => throw new NotImplementedException();

        /// <inheritdoc/>
        public override string Operation => throw new NotImplementedException();

        /// <inheritdoc/>
        public override string Syntax => throw new NotImplementedException();

        /// <inheritdoc/>
        public override string Assembly => throw new NotImplementedException();

        /// <inheritdoc/>
        public override Size Size
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
