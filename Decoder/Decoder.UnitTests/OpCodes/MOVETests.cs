using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.OpCodes
{
    [TestFixture]
    public class MOVETests
    {
        [TestCase(0, 1)]
        [TestCase(0, 2)]
        [TestCase(0, 3)]
        [TestCase(0, 4)]
        [TestCase(0, 5)]
        [TestCase(0, 6)]
        [TestCase(0, 7)]
        public void CheckMoveByteFromDtoD(byte dFrom, byte dTo)
        {
            // move byte Dx Dy
            // 0001 xxx0 0000 0xxx
            ushort code = 0x1000;

            code |= (ushort)(dTo << 9);
            code |= (ushort)(dFrom << 0);

            MachineState state = new MachineState(new Data(new byte[] { (byte)((code >> 8) & 0xFF), (byte)(code >> 0 & 0xFF) }), 0x0000);
            state.FetchOpCode();

            byte expectedVal = 0xAA;

            state.WriteDReg(dFrom, expectedVal);

            var opcode = new MOVE(state);

            byte actualVal = (byte)state.ReadDReg(dTo);

            Assert.That(actualVal, Is.EqualTo(expectedVal));
            Assert.That(state.PC, Is.EqualTo(0x2));
        }
    }
}
