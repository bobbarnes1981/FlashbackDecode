using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.Flashback
{
    [TestFixture]
    public class ROMSpecificTests
    {
        [Test]
        public void TEST_0x200()
        {
            // 0011000111111100     0x31FC
            // 0000000000010111     0x0017
            // 1111100000011010     0xF416

            // move #23 to 0xF416

            byte[] data = new byte[]
            {
                0x31, 0xFC, 0x00, 0x17, 0xF4, 0x16
            };

            MachineState state = new MachineState(new Data(data), 0x0000);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            byte actualData = (byte)state.Read(0xF416);

            Assert.That(actualData, Is.EqualTo(23));
            Assert.That(state.PC, Is.EqualTo(0x6));
        }

        [Test]
        public void TEST_0x206()
        {
            // 0110000000000000     0x6000
            // 0000000101100110     0x0166

            // Branch Always (W) 358 to 0xF416

            byte[] data = new byte[]
            {
                0x60, 0x00, 0x01, 0x66
            };

            MachineState state = new MachineState(new Data(data), 0x0000);
            state.FetchOpCode();

            var opcode = new BRA(state);

            Assert.That(state.PC, Is.EqualTo(360));
        }
    }
}
