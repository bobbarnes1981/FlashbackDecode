using Decoder.M68k.Enums;
using Decoder.M68k.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.M68k.OpCodes.MOVEATests
{
    [TestFixture]
    class MOVEAByteTests
    {
        // Dn
        // An
        // (An)
        // (An)+
        // -(An)
        // (d16,An)
        // (d8,An,Xn)
        // (xxx).W
        // (xxx).L
        // #<data>
        // (d16,PC)
        // (d8,PC,Xn)

        public void TEST()
        {
            // 00ssaaa001mmmxxx
            // MOVEM <ea>,An

            byte[] data = new byte[]
            {
                0x00, 0x00
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVEA(state);

            Assert.That(opcode.Assembly, Is.EqualTo($"MOVEA ,An"));
            Assert.That(opcode.Size, Is.EqualTo(Size.Word));

        }
    }
}
