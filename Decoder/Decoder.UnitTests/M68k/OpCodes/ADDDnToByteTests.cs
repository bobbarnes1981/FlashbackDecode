using Decoder.Exceptions;
using Decoder.M68k.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.M68k.OpCodes
{
    [TestFixture]
    class ADDDnToByteTests
    {
        [Test(Description = "ADD.B D1,D0")]
        public void CheckAddD1ToD0()
        {
            // ADD Dn,<ea>
            // 1101 dddD ssmm mxxx
            // 1101 0011 0000 0000  0xD300
            // ADD D1,D0

            byte[] data = new byte[]
            {
                0xD3, 0x00
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x0, 0x22221111);
            state.WriteDReg(0x1, 0x33332222);
            state.FetchOpCode();

            Assert.Throws<InvalidOpCodeException>(() => new ADD(state));
        }

        [Test(Description = "ADD.B D1,A0")]
        public void CheckAddD1ToA0()
        {
            // ADD Dn,<ea>
            // 1101 dddD ssmm mxxx
            // 1101 0011 0000 1000  0xD308
            // ADD.B D1,A0

            byte[] data = new byte[]
            {
                0xD3, 0x08
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x0, 0x22221111);
            state.WriteAReg(0x1, 0x33332222);
            state.FetchOpCode();

            Assert.Throws<InvalidOpCodeException>(() => new ADD(state));
        }

        [Test(Description = "ADD.B D1,(A0)")]
        public void CheckAddD1To_A0_()
        {
            // ADD Dn,<ea>
            // 1101 dddD ssmm mxxx
            // 1101 0011 0001 0000  0xD310
            // ADD D1,(A0)

            byte[] data = new byte[]
            {
                0xD3, 0x10
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x1, 0x22221111);
            state.WriteAReg(0x0, 0x00FF0000);
            state.WriteByte(0x00FF0000, 0x22);
            state.FetchOpCode();

            var opcode = new ADD(state);

            Assert.That(opcode.Assembly, Is.EqualTo("ADD.B D1,(A0)"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x22221111));
            Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x33));
        }

        [Test(Description = "ADD.B D1,(A0)+")]
        public void CheckAddD1To_A0i_()
        {
            // ADD Dn,<ea>
            // 1101 dddD ssmm mxxx
            // 1101 0011 0001 1000  0xD318
            // ADD D1,(A0)+

            byte[] data = new byte[]
            {
                0xD3, 0x18
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x1, 0x22221111);
            state.WriteAReg(0x0, 0x00FF0000);
            state.WriteByte(0x00FF0000, 0x22);
            state.FetchOpCode();

            var opcode = new ADD(state);

            Assert.That(opcode.Assembly, Is.EqualTo("ADD.B D1,(A0)+"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0001));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x22221111));
            Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x33));
        }

        [Test(Description = "ADD.B D1,-(A0)")]
        public void CheckAddD1To_dA0_()
        {
            // ADD Dn,<ea>
            // 1101 dddD ssmm mxxx
            // 1101 0011 0010 0000  0xD320
            // ADD D1,(A0)+

            byte[] data = new byte[]
            {
                0xD3, 0x20
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x1, 0x22221111);
            state.WriteAReg(0x0, 0x00FF0001);
            state.WriteByte(0x00FF0000, 0x22);
            state.FetchOpCode();

            var opcode = new ADD(state);

            Assert.That(opcode.Assembly, Is.EqualTo("ADD.B D1,-(A0)"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x22221111));
            Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x33));
        }

        //to
        // NOT Dn
        // NOT An
        // (An)
        // (An)+
        // -(An)
        // (d16, An)
        // (d8,An,Xn)
        // (xxx).W
        // (xxx}.L
        // NOT #<data>
        // NOT (d16,PC)
        // NOT (d8,PC,Xn)

    }
}
