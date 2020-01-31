using Decoder.Exceptions;
using Decoder.M68k.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.M68k.OpCodes.ANDITests
{
    [TestFixture]
    class ANDIByteTests
    {
        [Test(Description = "ANDI #10,D1")]
        public void CheckAndDn()
        {
            // ANDI #<data>,<ea>
            // 0000 0010 0000 0001  0x0201
            // 0000 0000 0000 0101  0x000A
            // ANDI #10,D1

            byte[] data = new byte[]
            {
                0x02, 0x01, 0x00, 0x0A
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x1, 0xFF);
            state.FetchOpCode();

            var opcode = new ANDI(state);

            Assert.That(opcode.Assembly, Is.EqualTo("ANDI #10,D1"));
            Assert.That(state.PC, Is.EqualTo(0x04));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x0000000A));
        }

        [Test(Description = "ANDI #10,A1")]
        public void CheckAndAn()
        {
            // ANDI #<data>,<ea>
            // 0000 0010 0000 1001  0x0209
            // 0000 0000 0000 0101  0x000A
            // ANDI #10,A1

            byte[] data = new byte[]
            {
                0x02, 0x09, 0x00, 0x0A
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x1, 0xFF);
            state.FetchOpCode();

            Assert.Throws<InvalidOpCodeException>(() => new ANDI(state));
        }

        [Test(Description = "ANDI #10,(A1)")]
        public void CheckAnd_An_()
        {
            // ANDI #<data>,<ea>
            // 0000 0010 0001 0001  0x0211
            // 0000 0000 0000 0101  0x000A
            // ANDI #10,(A1)

            byte[] data = new byte[]
            {
                0x02, 0x11, 0x00, 0x0A
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x1, 0x00FF0000);
            state.WriteByte(0x00FF0000, 0xFF);
            state.WriteByte(0x00FF0002, 0xAA);
            state.WriteDReg(0x1, 0xFF);
            state.FetchOpCode();

            var opcode = new ANDI(state);

            Assert.That(opcode.Assembly, Is.EqualTo("ANDI #10,(A1)"));
            Assert.That(state.PC, Is.EqualTo(0x04));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00FF0000));
            Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x0000000A));
        }

        [Test(Description = "ANDI #10,(A1)+")]
        public void CheckAnd_Ani_()
        {
            // ANDI #<data>,<ea>
            // 0000 0010 0001 1001  0x0219
            // 0000 0000 0000 0101  0x000A
            // ANDI #10,(A1)

            byte[] data = new byte[]
            {
                0x02, 0x19, 0x00, 0x0A
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x1, 0x00FF0000);
            state.WriteByte(0x00FF0000, 0xFF);
            state.WriteByte(0x00FF0002, 0xAA);
            state.WriteDReg(0x1, 0xFF);
            state.FetchOpCode();

            var opcode = new ANDI(state);

            Assert.That(opcode.Assembly, Is.EqualTo("ANDI #10,(A1)+"));
            Assert.That(state.PC, Is.EqualTo(0x04));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00FF0002));
            Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x0000000A));
        }

        // -(An)
        // d16,An
        // d8,An,Xn
        // (xxx).w
        // (xxx).l
        // not immediate
        // not d16,PC
        // not d8,PC,Xn
    }
}
