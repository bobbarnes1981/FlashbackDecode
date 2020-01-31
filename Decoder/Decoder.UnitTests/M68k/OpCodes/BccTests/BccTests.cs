using Decoder.M68k.Enums;
using Decoder.M68k.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.M68k.OpCodes.BccTests
{
    [TestFixture]
    class BccTests
    {
        [Test(Description = "BNE $0008")]
        public void CheckBNEBranchWhenNotEqualToZero()
        {

            // BNE <label>
            // 0110 0110 0000 0110  0x6606
            // BNE $0008

            byte[] data = new byte[]
            {
                0x66, 0x06
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.Condition_Z = false;
            state.FetchOpCode();

            var opcode = new Bcc(state);

            Assert.That(opcode.Assembly, Is.EqualTo("BNE $0008"));
            Assert.That(opcode.Size, Is.EqualTo(Size.Byte));
            Assert.That(state.PC, Is.EqualTo(0x00000008));
        }

        [Test(Description = "BNE $0008")]
        public void CheckBNENotBranchWhenEqualToZero()
        {
            // BNE <label>
            // 0110 0110 0000 0110  0x6606
            // BNE 6

            byte[] data = new byte[]
            {
                0x66, 0x06
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.Condition_Z = true;
            state.FetchOpCode();

            var opcode = new Bcc(state);

            Assert.That(opcode.Assembly, Is.EqualTo("BNE $0008"));
            Assert.That(opcode.Size, Is.EqualTo(Size.Byte));
            Assert.That(state.PC, Is.EqualTo(0x00000002));
        }
    }
}
