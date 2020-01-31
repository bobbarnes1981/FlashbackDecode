using System;

namespace Decoder.M68k.OpCodes
{
    //class DBcc : OpCode
    //{
    //    protected override string definition => "0101cccc11001ddd";

    //    private short displacement;

    //    public override string Name => "DBcc";

    //    public override string Description => "Test Condition, Decrement and Branch";

    //    public override string Operation => "IF CONDITON FALSE (Dn–1 -> Dn; If Dn != – 1 Then PC + dn -> PC)";

    //    public override string Syntax => string.Format("{0} Dn, <label>", Name);

    //    public override string Assembly => string.Format("{0}.{1} {2}, 0x{3:X4}", FullName, getCondition(), getDn(), displacement);

    //    public DBcc(MachineState state)
    //        : base(state)
    //    {
    //        displacement = (short)readData(Size); // always WORD
    //    }

    //    protected Condition getCondition()
    //    {
    //        return (Condition)getSizeFromBits1('c');
    //    }

    //    protected override Size getSize()
    //    {
    //        return Size.Word;
    //    }
    //}
}
