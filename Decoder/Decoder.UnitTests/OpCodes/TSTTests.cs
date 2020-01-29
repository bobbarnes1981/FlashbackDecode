using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.OpCodes
{
    [TestFixture]
    public class TSTTests
    {
        [Test]
        public void CheckIsNegativeSetsN()
        {
            // TST <ea>
            // 0100 1010 1011 1001  0x4AB9
            // 0000 0000 1111 1111  0x00FF
            // 0000 0000 0000 0000  0x0000

            // TST 0x00FF0000

            byte[] data = new byte[]
            {
                0x4A, 0xB9, 0x00, 0xFF, 0x00, 0x00
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteByte(0xFF0000, 0x80);
            state.FetchOpCode();

            state.SR |= 0x0011;

            var opcode = new TST(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Long));

            Assert.That(state.PC, Is.EqualTo(0x00000006));

            Assert.That(state.Condition_X, Is.True);    // not affected
            Assert.That(state.Condition_N, Is.True);
            Assert.That(state.Condition_Z, Is.False);
            Assert.That(state.Condition_V, Is.False);   // always cleared
            Assert.That(state.Condition_C, Is.False);   // always cleared
        }

        [Test]
        public void CheckIsZeroSetsZ()
        {
            // TST <ea>
            // 0100 1010 1011 1001  0x4AB9
            // 0000 0000 1111 1111  0x00FF
            // 0000 0000 0000 0000  0x0000

            // TST 0x00FF0000

            byte[] data = new byte[]
            {
                0x4A, 0xB9, 0x00, 0xFF, 0x00, 0x00
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteByte(0xFF0000, 0x00);
            state.FetchOpCode();

            state.SR |= 0x0011;

            var opcode = new TST(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Long));

            Assert.That(state.PC, Is.EqualTo(0x00000006));

            Assert.That(state.Condition_X, Is.True);    // not affected
            Assert.That(state.Condition_N, Is.False);
            Assert.That(state.Condition_Z, Is.True);
            Assert.That(state.Condition_V, Is.False);   // always cleared
            Assert.That(state.Condition_C, Is.False);   // always cleared
        }
    }
}
