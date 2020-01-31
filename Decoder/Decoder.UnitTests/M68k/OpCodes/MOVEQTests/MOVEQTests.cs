using Decoder.M68k.Enums;
using Decoder.M68k.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.M68k.OpCodes.MOVEQTests
{
    [TestFixture]
    class MOVEQTests
    {
        [TestCase(0x01, DataRegister.D0)]
        [TestCase(0x02, DataRegister.D1)]
        [TestCase(0x03, DataRegister.D2)]
        [TestCase(0x04, DataRegister.D3)]
        [TestCase(0x05, DataRegister.D4)]
        [TestCase(0x06, DataRegister.D5)]
        [TestCase(0x07, DataRegister.D6)]
        [TestCase(0x08, DataRegister.D7)]
        public void CheckMoveNumberToDRegister(byte number, DataRegister register)
        {
            // 0111ddd0bbbbbbbb
            // MOVEQ #0,D0

            byte hi = 0x70;
            byte lo = number;

            hi |= (byte)((byte)register << 1);

            byte[] data = new byte[]
            {
                hi, lo
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x5, 0xFF0000);
            for (int i = 0; i < 16; i++)
            {
                state.WriteByte((uint)(0xFF0000 + i), (byte)(0x00 + i));
            }
            state.FetchOpCode();

            var opcode = new MOVEQ(state);

            Assert.That(opcode.Assembly, Is.EqualTo($"MOVEQ #{number},{register}"));
            Assert.That(opcode.Size, Is.EqualTo(Size.Long));
            Assert.That(state.ReadDReg((byte)register), Is.EqualTo(number));
        }
    }
}
