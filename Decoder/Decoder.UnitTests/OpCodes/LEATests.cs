using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.OpCodes
{
    [TestFixture]
    class LEATests
    {
        [Test]
        public void CheckLoadD16PCLoadsValue()
        {
            // LEA <ea>, An
            // 0100 1011 1111 1010  0x4BFA
            // 0000 0000 0111 1100  0x007C

            // LEA (d16, PC), A5
            // LEA (124, PC), A5

            byte[] data = new byte[]
            {
                0x4B, 0xFA, 0x00, 0x7C
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new LEA(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Long));

            Assert.That(state.PC, Is.EqualTo(0x00000004));
            Assert.That(state.ReadAReg(0x5), Is.EqualTo(0x0000007C));
        }
    }
}
