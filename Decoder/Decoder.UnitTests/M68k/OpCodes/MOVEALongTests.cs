using Decoder.M68k.Enums;
using Decoder.M68k.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.M68k.OpCodes
{
    [TestFixture]
    class MOVEALongTests
    {
        [Test(Description = "MOVEA D0,A1")]
        public void CheckMoveD0ToA1()
        {
            // 00ss aaa0 01mm mxxx
            // 0010 0010 0100 0000  0x2240
            // MOVEM <ea>,An
            // MOVEM D0,A1

            byte[] data = new byte[]
            {
                0x22, 0x40
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x0, 0xAABBCCDD);
            state.FetchOpCode();

            var opcode = new MOVEA(state);

            Assert.That(opcode.Assembly, Is.EqualTo($"MOVEA D0,A1"));
            Assert.That(opcode.Size, Is.EqualTo(Size.Long));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0xAABBCCDD));
        }

        [Test(Description = "MOVEA A0,A1")]
        public void CheckMoveA0ToA1()
        {
            // 00ss aaa0 01mm mxxx
            // 0010 0010 0100 1000  0x2248
            // MOVEM <ea>,An
            // MOVEM A0,A1

            byte[] data = new byte[]
            {
                0x22, 0x48
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x0, 0xAABBCCDD);
            state.FetchOpCode();

            var opcode = new MOVEA(state);

            Assert.That(opcode.Assembly, Is.EqualTo($"MOVEA A0,A1"));
            Assert.That(opcode.Size, Is.EqualTo(Size.Long));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0xAABBCCDD));
        }

        [Test(Description = "MOVEA (A0),A1")]
        public void CheckMove_A0_ToA1()
        {
            // 00ss aaa0 01mm mxxx
            // 0010 0010 0101 0000  0x2250
            // MOVEM <ea>,An
            // MOVEM (A0),A1

            byte[] data = new byte[]
            {
                0x22, 0x50
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteByte(0x00FF0000, 0xAA);
            state.WriteByte(0x00FF0001, 0xBB);
            state.WriteByte(0x00FF0002, 0xCC);
            state.WriteByte(0x00FF0003, 0xDD);
            state.WriteAReg(0x0, 0x00FF0000);
            state.FetchOpCode();

            var opcode = new MOVEA(state);

            Assert.That(opcode.Assembly, Is.EqualTo($"MOVEA (A0),A1"));
            Assert.That(opcode.Size, Is.EqualTo(Size.Long));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0xAABBCCDD));
        }

        [Test(Description = "MOVEA (A0)+,A1")]
        public void CheckMove_A0i_ToA1()
        {
            // 00ss aaa0 01mm mxxx
            // 0010 0010 0101 1000  0x2258
            // MOVEM <ea>,An
            // MOVEM (A0)+,A1

            byte[] data = new byte[]
            {
                0x22, 0x58
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteByte(0x00FF0000, 0xAA);
            state.WriteByte(0x00FF0001, 0xBB);
            state.WriteByte(0x00FF0002, 0xCC);
            state.WriteByte(0x00FF0003, 0xDD);
            state.WriteAReg(0x0, 0x00FF0000);
            state.FetchOpCode();

            var opcode = new MOVEA(state);

            Assert.That(opcode.Assembly, Is.EqualTo($"MOVEA (A0)+,A1"));
            Assert.That(opcode.Size, Is.EqualTo(Size.Long));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0004));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0xAABBCCDD));
        }

        [Test(Description = "MOVEA -(A0),A1")]
        public void CheckMove_dA0_ToA1()
        {
            // 00ss aaa0 01mm mxxx
            // 0010 0010 0110 0000  0x2260
            // MOVEM <ea>,An
            // MOVEM -(A0),A1

            byte[] data = new byte[]
            {
                0x22, 0x60
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteByte(0x00FF0000, 0xAA);
            state.WriteByte(0x00FF0001, 0xBB);
            state.WriteByte(0x00FF0002, 0xCC);
            state.WriteByte(0x00FF0003, 0xDD);
            state.WriteAReg(0x0, 0x00FF0004);
            state.FetchOpCode();

            var opcode = new MOVEA(state);

            Assert.That(opcode.Assembly, Is.EqualTo($"MOVEA -(A0),A1"));
            Assert.That(opcode.Size, Is.EqualTo(Size.Long));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0xAABBCCDD));
        }

        // (d16,An)
        // (d8,An,Xn)
        // (xxx).W
        // (xxx).L
        // #<data>
        // (d16,PC)
        // (d8,PC,Xn)
    }
}
