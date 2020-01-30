using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.OpCodes.MOVETests
{
    [TestFixture]
    class MOVEByteToD1Tests
    {
        [Test(Description = "MOVE.b D0,D1")]
        public void CheckMoveD0ToD1()
        {
            // MOVE <ea>,<ea>
            // 0001 0010 0000 0000  0x1200
            // MOVE D0,D1

            byte[] data = new byte[]
            {
                0x12, 0x00
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x0, 0xAABBCCDD);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.That(opcode.Assembly, Is.EqualTo("MOVE.b D0,D1"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x000000DD));
        }

        [Test(Description = "MOVE.b A0,D1")]
        public void CheckMoveA0ToD1()
        {
            // MOVE <ea>,<ea>
            // 0001 0010 0000 1000  0x1208
            // MOVE A0,D1

            byte[] data = new byte[]
            {
                    0x12, 0x08
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x0, 0xAABBCCDD);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.That(opcode.Assembly, Is.EqualTo("MOVE.b A0,D1"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x000000DD));
        }

        [Test(Description = "MOVE.b (A0),D1")]
        public void CheckMove_A0_ToD1()
        {
            // MOVE <ea>,<ea>
            // 0001 0010 0001 0000  0x1210
            // MOVE (A0), D1

            byte[] data = new byte[]
            {
                0x12, 0x10
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x0, 0x00FF0000);
            state.WriteByte(0x00FF0000, 0xAA);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.That(opcode.Assembly, Is.EqualTo("MOVE.b (A0),D1"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x000000AA));
        }

        [Test(Description = "MOVE.b (A0)+,D1")]
        public void CheckMove_A0i_ToD1()
        {
            // MOVE <ea>,<ea>
            // 0001 0010 0001 1000  0x1218
            // MOVE (A0)+,D1

            byte[] data = new byte[]
            {
                0x12, 0x18
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x0, 0x00FF0000);
            state.WriteByte(0x00FF0000, 0xAA);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.That(opcode.Assembly, Is.EqualTo("MOVE.b (A0)+,D1"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0004));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x000000AA));
        }

        [Test(Description = "MOVE.b -(A0),D1")]
        public void CheckMove_dA0_ToD1()
        {
            // MOVE <ea>,<ea>
            // 0001 0010 0010 0000  0x1220
            // MOVE -(A0),D1

            byte[] data = new byte[]
            {
                0x12, 0x20
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x0, 0x00FF0002);
            state.WriteByte(0x00FF0000, 0xAA);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.That(opcode.Assembly, Is.EqualTo("MOVE.b -(A0),D1"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x000000AA));
        }

        [Test(Description = "MOVE.b ($0002,A0),D1")]
        public void CheckMove_d16A0_ToD1()
        {
            // MOVE <ea>,<ea>
            // 0001 0010 0010 1000  0x1228
            // 0000 0000 0000 0002  0x0002
            // MOVE (d16,A0),D1

            byte[] data = new byte[]
            {
                0x12, 0x28, 0x00, 0x02
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x0, 0x00FF0000);
            state.WriteByte(0x00FF0002, 0xAA);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.That(opcode.Assembly, Is.EqualTo("MOVE.b ($0002,A0),D1"));
            Assert.That(state.PC, Is.EqualTo(0x04));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x000000AA));
        }

        [Test(Description = "MOVE.b ($0002,PC),D1")]
        public void CheckMove_d16PC_ToD1()
        {
            // MOVE <ea>,<ea>
            // 0001 0010 0011 1010  0x123A
            // 0000 0000 0000 0002  0x0002
            // 1010 1010            0xAA
            // MOVE (d16,PC), D1

            byte[] data = new byte[]
            {
                0x12, 0x3A, 0x00, 0x02, 0xAA
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.That(opcode.Assembly, Is.EqualTo("MOVE.b ($0002,PC),D1"));
            Assert.That(state.PC, Is.EqualTo(0x04));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x000000AA));
        }

        [Test(Description = "MOVE.b #2,D1")]
        public void CheckMoveImmToD1()
        {
            // MOVE <ea>,<ea>
            // 0001 0010 0011 1100  0x123C
            // 0000 0000 0000 0002  0x0002
            // MOVE #2,D1

            byte[] data = new byte[]
            {
                0x12, 0x3C, 0x00, 0x02
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.That(opcode.Assembly, Is.EqualTo("MOVE.b #2,D1"));
            Assert.That(state.PC, Is.EqualTo(0x04));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x00000002));
        }
    }
}
