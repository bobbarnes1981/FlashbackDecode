namespace Decoder.OpCodes
{
    // Register rotate
    //class ROd : OpCode
    //{
    //    public override string Name => "RO";

    //    public override string Description => "Rotate (Without Extend) [Register]";

    //    public override string Operation => "Destination rotated by <count> -> Destination";

    //    public override string Syntax => "ROd Dx, Dy\r\nROd #<data>, Dy\r\nROd <ea>\r\nWhere d is direction L or R.";

    //    public override string Assembly => string.Format("RO{0} {1}{2}, {3}", getDirection().ToString()[0], getRString(), getR(), getDn());

    //    protected override string definition => "1110rrrDssM11ddd";

    //    public ROd(MachineState state)
    //        : base(state)
    //    {
    //    }

    //    protected override Size getSize()
    //    {
    //        return (Size)getBits('s');
    //    }

    //    protected Direction getDirection()
    //    {
    //        return (Direction)getBits('D');
    //    }

    //    protected Rotation getRotation()
    //    {
    //        return (Rotation)getBits('M');
    //    }

    //    protected string getRString()
    //    {
    //        switch (getRotation())
    //        {
    //            case Rotation.Immediate:
    //                return "#";

    //            case Rotation.Register:
    //                return "D";

    //            default:
    //                throw new System.Exception();
    //        }
    //    }

    //    protected ushort getR()
    //    {
    //        return getBits('r');
    //    }
    //}
}
