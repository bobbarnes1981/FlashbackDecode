using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.OpCodes
{
    [TestFixture]
    class MOVEMTests
    {
        [Test]
        public void CheckA5PostIncMovedToReg()
        {
            Assert.Fail("TODO");

            // MOVEM <ea>, <ea>
            // 0100 1100 1001 1101  0x4C9D

            // LEA (d16, PC), A5 ???
            // LEA (124, PC), A5 ???

            byte[] data = new byte[]
            {
                0x4C, 0x9D
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000);
            state.FetchOpCode();

            var opcode = new MOVEM(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Long));

            Assert.That(state.PC, Is.EqualTo(0x00000004));
            Assert.That(state.ReadAReg(0x5), Is.EqualTo(0x0000007C));
        }
    }
}
