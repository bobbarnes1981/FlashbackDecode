using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.OpCodes
{
    [TestFixture]
    class MOVETests
    {
        //dst not An, Immediate, d16pc, d8pcXn
        //src all allowed

        // DataReg          000 REG
        //Address Reg       001 REG
        //Address           010 REG
        // Address+         011 REG
        //-Address          100 REG
        // d16, an          101 REG
        // d16 PC           111 010
        // xxx.W (short)    111 000
        // xxx.l (long)     111 001
        // immediate        111 100

        [Test]
        public void CheckMoveD0ToA1()
        {
            // MOVEM <ea>, <ea>
            // 0001 0010 0100 0000  0x1240

            // MOVE D0, A1

            byte[] data = new byte[]
            {
                0x12, 0x40
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.Fail("write test");
        }

        [Test]
        public void CheckMoveA0ToA1()
        {
            // MOVEM <ea>, <ea>
            // 0001 0010 0100 1000  0x1248

            // MOVE A0, A1

            byte[] data = new byte[]
            {
                0x12, 0x48
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.Fail("write test");
        }

        [Test]
        public void CheckMove_A0_ToA1()
        {
            // MOVEM <ea>, <ea>
            // 0001 0010 0101 0000  0x1250

            // MOVE (A0), A1

            byte[] data = new byte[]
            {
                0x12, 0x50
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.Fail("write test");
        }

        [Test]
        public void CheckMove_A0i_ToA1()
        {
            // MOVEM <ea>, <ea>
            // 0001 0010 0101 1000  0x1258

            // MOVE (A0)+, A1

            byte[] data = new byte[]
            {
                0x12, 0x58
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.Fail("write test");
        }

        [Test]
        public void CheckMove_dA0_ToA1()
        {
            // MOVEM <ea>, <ea>
            // 0001 0010 0110 0000  0x1260

            // MOVE -(A0), A1

            byte[] data = new byte[]
            {
                0x12, 0x60
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.Fail("write test");
        }

        [Test]
        public void CheckMove_d16A0_ToA1()
        {
            // MOVEM <ea>, <ea>
            // 0001 0010 0110 1000  0x1258

            // MOVE (d16, A0), A1

            byte[] data = new byte[]
            {
                0x12, 0x58
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.Fail("write test");
        }

        [Test]
        public void CheckMove_aW_ToA1()
        {
            // MOVEM <ea>, <ea>
            // 0001 0010 0111 1000  0x1278

            // MOVE 0x0000, A1

            byte[] data = new byte[]
            {
                0x12, 0x78
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.Fail("write test");
        }

        [Test]
        public void CheckMove_aL_ToA1()
        {
            // MOVEM <ea>, <ea>
            // 0001 0010 0111 1001  0x1279

            // MOVE 0x00000000, A1

            byte[] data = new byte[]
            {
                0x12, 0x79
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.Fail("write test");
        }

        [Test]
        public void CheckMove_d16PC_ToA1()
        {
            // MOVEM <ea>, <ea>
            // 0001 0010 0111 1010  0x127A

            // MOVE (d16, PC), A1

            byte[] data = new byte[]
            {
                0x12, 0x7A
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.Fail("write test");
        }

        [Test]
        public void CheckMoveImmToA1()
        {
            // MOVEM <ea>, <ea>
            // 0001 0010 0111 1100  0x127C

            // MOVE #0, A1

            byte[] data = new byte[]
            {
                0x12, 0x7C
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new MOVE(state);

            Assert.Fail("write test");
        }

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

            Assert.That(state.ReadDReg(0x0), Is.EqualTo(0x0000BB));
        }
    }
}
