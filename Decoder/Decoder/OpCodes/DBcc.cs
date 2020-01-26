using System;

namespace Decoder.OpCodes
{
    class DBcc : OpCode
    {
        private short displacement;

        public override string Name => "DBcc";

        public override string Description => "Test Condition, Decrement and Branch";

        public override string Operation => "IF CONDITON FALSE (Dn–1 -> Dn; If Dn != – 1 Then PC + dn -> PC)";

        public override string Syntax => string.Format("{0} Dn, <label>", Name);

        public override string Assembly => string.Format("{0}.{1} {2}, 0x{3:X4}", FullName, getCondition(), getDn(), displacement);

        public DBcc(Data data, int address, ushort code)
            : base(data, address, code)
        {
            displacement = (short)readData(Size); // always WORD
        }

        protected Condition getCondition()
        {
            return (Condition)code.GetBits(8, 4);
        }

        protected override AddressRegister getAn()
        {
            throw new NotImplementedException();
        }

        protected override DataRegister getDn()
        {
            return (DataRegister)code.GetBits(0, 3);
        }

        protected override byte getM()
        {
            throw new NotImplementedException();
        }

        protected override Size getSize()
        {
            return Size.Word;
        }

        protected override byte getXn()
        {
            throw new NotImplementedException();
        }
    }
}
