using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.OpCodes
{
    [TestFixture]
    class MOVETests
    {
        //dst not [An, Immediate, d16pc, d8pcXn]
        //src all allowed

        // DataReg          000 REG
        // Address Reg      001 REG
        // Address          010 REG
        // Address+         011 REG
        //-Address          100 REG
        // d16, an          101 REG
        // d16 PC           111 010
        // xxx.W (short)    111 000
        // xxx.l (long)     111 001
        // immediate        111 100

        /// <summary>
        /// These should be invalid... Move to MOVE tests
        /// </summary>
        class ByteToA1
        {
            [Test]
            public void CheckMoveD0ToA1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0100 0000  0x1240

                // MOVE D0, A1

                byte[] data = new byte[]
                {
                0x12, 0x40
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteDReg(0x0, 0xAABBCCDD);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0xAABBCCDD));
            }

            [Test]
            public void CheckMoveA0ToA1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0100 1000  0x1248

                // MOVE A0, A1

                byte[] data = new byte[]
                {
                0x12, 0x48
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x0, 0xAABBCCDD);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0xAABBCCDD));
            }

            [Test]
            public void CheckMove_A0_ToA1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0101 0000  0x1250

                // MOVE (A0), A1

                byte[] data = new byte[]
                {
                0x12, 0x50
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x0, 0x00FF0000);
                state.WriteByte(0x00FF0000, 0xAA);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x000000AA));
            }

            [Test]
            public void CheckMove_A0i_ToA1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0101 1000  0x1258

                // MOVE (A0)+, A1

                byte[] data = new byte[]
                {
                0x12, 0x58, 0x00, 0xFF, 0x00, 0x00
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x0, 0x00FF0000);
                state.WriteByte(0x00FF0000, 0xAA);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0004));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x000000AA));
            }

            [Test]
            public void CheckMove_dA0_ToA1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0110 0000  0x1260

                // MOVE -(A0), A1

                byte[] data = new byte[]
                {
                0x12, 0x60
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x0, 0x00FF0002);
                state.WriteByte(0x00FF0000, 0xAA);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x000000AA));
            }

            [Test]
            public void CheckMove_d16A0_ToA1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0110 1000  0x1268
                // 0000 0000 0000 0002  0x0002

                // MOVE (d16, A0), A1

                byte[] data = new byte[]
                {
                0x12, 0x68, 0x00, 0x02
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x0, 0x00FF0000);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x04));
                Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00FF0002));
            }

            [Test]
            public void CheckMove_d16PC_ToA1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0111 1010  0x127A
                // 0000 0000 0000 0002  0x0002

                // MOVE (d16, PC), A1

                byte[] data = new byte[]
                {
                0x12, 0x7A, 0x00, 0x02
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x04));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00000004));
            }

            [Test]
            public void CheckMoveImmToA1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0111 1100  0x127C
                // 0000 0000 0000 0002  0x0002

                // MOVE #2, A1

                byte[] data = new byte[]
                {
                0x12, 0x7C, 0x00, 0x02
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x04));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00000002));
            }
        }

        /// <summary>
        /// Byte To D1
        /// </summary>
        class ByteToD1
        {
            [Test]
            public void CheckMoveD0ToD1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0000 0000  0x1200

                // MOVE D0, D1

                byte[] data = new byte[]
                {
                    0x12, 0x00
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteDReg(0x0, 0xAABBCCDD);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x000000DD));
            }

            [Test]
            public void CheckMoveA0ToD1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0000 1000  0x1208

                // MOVE A0, D1

                byte[] data = new byte[]
                {
                    0x12, 0x08
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x0, 0xAABBCCDD);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x000000DD));
            }

            [Test]
            public void CheckMove_A0_ToD1()
            {
                // MOVE <ea>, <ea>
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

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x000000AA));
            }

            [Test]
            public void CheckMove_A0i_ToD1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0001 1000  0x1218

                // MOVE (A0)+, D1

                byte[] data = new byte[]
                {
                    0x12, 0x18
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x0, 0x00FF0000);
                state.WriteByte(0x00FF0000, 0xAA);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0004));
                Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x000000AA));
            }

            [Test]
            public void CheckMove_dA0_ToD1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0010 0000  0x1220

                // MOVE -(A0), D1

                byte[] data = new byte[]
                {
                    0x12, 0x20
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x0, 0x00FF0002);
                state.WriteByte(0x00FF0000, 0xAA);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
                Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x000000AA));
            }

            [Test]
            public void CheckMove_d16A0_ToD1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0010 1000  0x1228
                // 0000 0000 0000 0002  0x0002

                // MOVE (d16, A0), D1

                byte[] data = new byte[]
                {
                    0x12, 0x28, 0x00, 0x02
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x0, 0x00FF1234);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x04));
                Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF1234));
                Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x00000036));
            }

            [Test]
            public void CheckMove_d16PC_ToD1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0011 1010  0x123A
                // 0000 0000 0000 0002  0x0002

                // MOVE (d16, PC), D1

                byte[] data = new byte[]
                {
                    0x12, 0x3A, 0x00, 0x02
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x04));
                Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x00000004));
            }

            [Test]
            public void CheckMoveImmToD1()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 0011 1100  0x123C
                // 0000 0000 0000 0002  0x0002

                // MOVE #2, D1

                byte[] data = new byte[]
                {
                    0x12, 0x3C, 0x00, 0x02
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x04));
                Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x00000002));
            }
        }

        class ByteTo_A1_
        {
            [Test]
            public void CheckMoveD0To_A1_()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 1000 0000  0x1280

                // MOVE D0, (A1)

                byte[] data = new byte[]
                {
                    0x12, 0x80
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x1, 0x00FF0000);
                state.WriteDReg(0x0, 0xAABBCCDD);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00FF0000));
                Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x000000DD));
            }

            [Test]
            public void CheckMoveA0To_A1_()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 1000 1000  0x1288

                // MOVE A0, (A1)

                byte[] data = new byte[]
                {
                    0x12, 0x88
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x1, 0x00FF0000);
                state.WriteAReg(0x0, 0xAABBCCDD);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00FF0000));
                Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x000000DD));
            }

            [Test]
            public void CheckMove_A0_To_A1_()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 1001 0000  0x1290

                // MOVE (A0), (A1)

                byte[] data = new byte[]
                {
                    0x12, 0x90
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x1, 0x00FF0000);
                state.WriteAReg(0x0, 0x00FF0002);
                state.WriteByte(0x00FF0000, 0xAA);
                state.WriteByte(0x00FF0002, 0xBB);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00FF0000));
                Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0002));
                Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x000000BB));
            }

            [Test]
            public void CheckMove_A0i_To_A1_()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 1001 1000  0x1298

                // MOVE (A0)+, (A1)

                byte[] data = new byte[]
                {
                    0x12, 0x98
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x1, 0x00FF0000);
                state.WriteAReg(0x0, 0x00FF0002);
                state.WriteByte(0x00FF0000, 0xAA);
                state.WriteByte(0x00FF0002, 0xBB);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00FF0000));
                Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0006));
                Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x000000BB));
            }

            [Test]
            public void CheckMove_dA0_To_A1_()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 1010 0000  0x12A0

                // MOVE -(A0), (A1)

                byte[] data = new byte[]
                {
                    0x12, 0xA0
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x1, 0x00FF0004);
                state.WriteAReg(0x0, 0x00FF0002);
                state.WriteByte(0x00FF0000, 0xAA);
                state.WriteByte(0x00FF0002, 0xBB);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x02));
                Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00FF0004));
                Assert.That(state.ReadByte(0x00FF0004), Is.EqualTo(0x000000AA));
            }

            [Test]
            public void CheckMove_d16A0_To_A1_()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 1010 1000  0x12A8
                // 0000 0000 0000 0002  0x0002

                // MOVE (d16, A0), (A1)

                byte[] data = new byte[]
                {
                    0x12, 0xA8, 0x00, 0x02
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x1, 0x00FF0000);
                state.WriteAReg(0x0, 0x00FF0002);
                state.WriteByte(0x00FF0000, 0xAA);
                state.WriteByte(0x00FF0002, 0xBB);
                state.WriteByte(0x00FF0004, 0xCC);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x04));
                Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0002));
                Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00FF0000));
                Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x00000004)); // is this right?
            }

            [Test]
            public void CheckMove_d16PC_To_A1_()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 1011 1010  0x12BA
                // 0000 0000 0000 0002  0x0002

                // MOVE (d16, PC), (A1)

                byte[] data = new byte[]
                {
                    0x12, 0xBA, 0x00, 0x02
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x1, 0x00FF0000);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x04));
                Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x00000004)); // is this right?
            }

            [Test]
            public void CheckMoveImmTo_A1_()
            {
                // MOVE <ea>, <ea>
                // 0001 0010 1011 1100  0x12BC
                // 0000 0000 0000 0002  0x0002

                // MOVE #2, (A1)

                byte[] data = new byte[]
                {
                    0x12, 0xBC, 0x00, 0x02
                };

                MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
                state.WriteAReg(0x1, 0x00FF0000);
                state.FetchOpCode();

                var opcode = new MOVE(state);

                Assert.That(state.PC, Is.EqualTo(0x04));
                Assert.That(state.ReadByte(0x00FF0000), Is.EqualTo(0x00000002));
            }
        }
    }
}
