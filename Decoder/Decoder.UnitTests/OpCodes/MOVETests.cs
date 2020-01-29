using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.OpCodes
{
    [TestFixture]
    class MOVETests
    {
        [Test]
        public void CheckMoveA1WithDisplacementToD0()
        {
            // MOVEM <ea>, <ea>
            // 0001 0000 0010 1001  0x1029
            // 1110 1111 0000 0001  0xEF01

            // MOVE (-4351, A1), D0

            byte[] data = new byte[]
            {
                0x10, 0x29, 0xEF, 0x01
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x1, 0xFF10FF);
            state.WriteByte(0xFF0000, 0xBB);
            state.WriteByte(0xFF0001, 0xAA);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.That(state.PC, Is.EqualTo(0x04));

            Assert.That(opcode.Size, Is.EqualTo(Size.Byte));

            Assert.That(state.ReadDReg(0x0), Is.EqualTo(0x00BB));
        }
    }
}
