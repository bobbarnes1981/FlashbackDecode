using System;

namespace Decoder.OpCodes
{
    class AND : OpCode
    {
        public override string Name => "AND";

        public override string Description => "AND Logical";

        public override string Operation => "Source L Destination -> Destination";

        public override string Syntax => string.Format("{0} <ea>, Dn\r\n{0} Dn, <ea>", Name);

        public override string Assembly
        {
            get
            {
                switch (getDirection())
                {
                    case LogicDirection.Dn_EA:
                        return string.Format("{0} {1}, {2}", Name, getDn(), getEAString(decodeEA(), EA));

                    case LogicDirection.EA_Dn:
                        return string.Format("{0} {2}, {1}", Name, getDn(), getEAString(decodeEA(), EA));

                    default:
                        throw new InvalidStateException();
                }
            }
        }

        protected override string definition => "1100dddDssmmmxxx";

        public AND(MachineState state)
            : base(state)
        {
            EA = readEA(decodeEA());
        }

        protected LogicDirection getDirection()
        {
            return (LogicDirection)getBits('D');
        }

        protected override Size getSize()
        {
            return (Size)getBits('s');
        }
    }
}
