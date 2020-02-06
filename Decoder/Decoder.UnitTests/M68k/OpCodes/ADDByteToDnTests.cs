using Decoder.Exceptions;
using Decoder.M68k.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.M68k.OpCodes
{
    [TestFixture]
    class ADDByteToDnTests
    {
        [Test(Description = "ADD.B D0,D1")]
        public void CheckAddD0ToD1()
        {
            // ADD Dn,<ea>
            // 1101 dddD ssmm mxxx
            // 1101 0010 0000 0000  0xD200
            // ADD.B D0,D1

            byte[] data = new byte[]
            {
                0xD2, 0x00
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x0, 0x22221111);
            state.WriteDReg(0x1, 0x33332222);
            state.FetchOpCode();

            var opcode = new ADD(state);

            Assert.That(opcode.Assembly, Is.EqualTo("ADD.B D0,D1"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadDReg(0x0), Is.EqualTo(0x22221111));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x00000033));
        }

        [Test(Description = "ADD.B A0,D1")]
        public void CheckAddA0ToD1()
        {
            // ADD Dn,<ea>
            // 1101 dddD ssmm mxxx
            // 1101 0010 0000 1000  0xD208
            // ADD A0,D1

            byte[] data = new byte[]
            {
                0xD2, 0x08
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x1, 0x22221111);
            state.WriteAReg(0x0, 0x33332222);
            state.FetchOpCode();

            Assert.Throws<InvalidOpCodeException>(() => new ADD(state));
        }

        [Test(Description = "ADD.B (A0),D1")]
        public void CheckAdd_A0_ToD1()
        {
            // ADD Dn,<ea>
            // 1101 dddD ssmm mxxx
            // 1101 0010 0001 0000  0xD210
            // ADD (A0),D1

            byte[] data = new byte[]
            {
                0xD2, 0x10
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x1, 0x22221111);
            state.WriteAReg(0x0, 0x00FF0000);
            state.WriteByte(0x00FF0000, 0x22);
            state.FetchOpCode();

            var opcode = new ADD(state);

            Assert.That(opcode.Assembly, Is.EqualTo("ADD.B (A0),D1"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x00000033));
            Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x22));
        }

        [Test(Description = "ADD.B (A0)+,D1")]
        public void CheckAdd_A0i_ToD1()
        {
            // ADD Dn,<ea>
            // 1101 dddD ssmm mxxx
            // 1101 0010 0001 1000  0xD218
            // ADD (A0)+,D1

            byte[] data = new byte[]
            {
                0xD2, 0x18
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x1, 0x22221111);
            state.WriteAReg(0x0, 0x00FF0000);
            state.WriteByte(0x00FF0000, 0x22);
            state.FetchOpCode();

            var opcode = new ADD(state);

            Assert.That(opcode.Assembly, Is.EqualTo("ADD.B (A0)+,D1"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0001));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x00000033));
            Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x22));
        }

        [Test(Description = "ADD.B -(A0),D1")]
        public void CheckAdd_dA0_ToD1()
        {
            // ADD Dn,<ea>
            // 1101 dddD ssmm mxxx
            // 1101 0010 0010 0000  0xD220
            // ADD -(A0),D1

            byte[] data = new byte[]
            {
                0xD2, 0x20
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x1, 0x22221111);
            state.WriteAReg(0x0, 0x00FF0001);
            state.WriteByte(0x00FF0000, 0x22);
            state.FetchOpCode();

            var opcode = new ADD(state);

            Assert.That(opcode.Assembly, Is.EqualTo("ADD.B -(A0),D1"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x00000033));
            Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x22));
        }

        //from
        // Dn
        // An (not for byte)
        // (An)
        // (An)+
        // -(An)
        // (d16, An)
        // (d8,AN,Xn)
        // (xxx).W
        // (xxx).L
        // #<data>
        // (d16,PC)
        // (d8,PC,Xn)
    }
}
