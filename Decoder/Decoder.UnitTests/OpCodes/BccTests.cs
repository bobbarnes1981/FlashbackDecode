using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.OpCodes
{
    [TestFixture]
    class BccTests
    {
        [Test]
        public void CheckBNEBranchWhenNotEqualToZero()
        {

            // BNE <label>
            // 0110 0110 0000 0110  0x6606

            // BNE 6

            byte[] data = new byte[]
            {
                0x66, 0x06
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000);
            state.Condition_Z = false;
            state.FetchOpCode();

            var opcode = new Bcc(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Byte));

            Assert.That(state.PC, Is.EqualTo(0x00000008));
        }

        [Test]
        public void CheckBNENotBranchWhenEqualToZero()
        {
            // BNE <label>
            // 0110 0110 0000 0110  0x6606

            // BNE 6

            byte[] data = new byte[]
            {
                0x66, 0x06
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000);
            state.Condition_Z = true;
            state.FetchOpCode();

            var opcode = new Bcc(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Byte));

            Assert.That(state.PC, Is.EqualTo(0x00000002));
        }
    }
}
