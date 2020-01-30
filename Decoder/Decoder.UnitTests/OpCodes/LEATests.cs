﻿using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.OpCodes
{
    [TestFixture]
    class LEATests
    {
        /// <summary>
        /// This should be invalid
        /// </summary>
        [Test]
        public void CheckLoadD0ToA1()
        {
            // LEA <ea>, An
            // 0100 0011 1100 0000  0x43C0

            // LEA D0, A1

            byte[] data = new byte[]
            {
                0x43, 0xC0
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x0, 0xAABBCCDD);
            state.FetchOpCode();

            var opcode = new LEA(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Long));

            Assert.That(state.PC, Is.EqualTo(0x00000002));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0xAABBCCDD));
        }

        // This should be invalid
        [Test]
        public void CheckLoadA0ToA1()
        {
            // LEA <ea>, An
            // 0100 0011 1100 1000  0x43C8

            // LEA A0, A1

            byte[] data = new byte[]
            {
                0x43, 0xC8
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x0, 0xAABBCCDD);
            state.FetchOpCode();

            var opcode = new LEA(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Long));

            Assert.That(state.PC, Is.EqualTo(0x00000002));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0xAABBCCDD));
        }

        [Test]
        public void CheckLoad_A0_ToA1()
        {
            // LEA <ea>, An
            // 0100 0011 1101 0000  0x43D0

            // LEA (A0), A1

            byte[] data = new byte[]
            {
                0x43, 0xD0
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x0, 0x00FF0000);
            state.WriteByte(0x00FF0000, 0xAA);
            state.FetchOpCode();

            var opcode = new LEA(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Long));

            Assert.That(state.PC, Is.EqualTo(0x00000002));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0xAA000000));
        }

        /// <summary>
        /// This should be invalid
        /// </summary>
        [Test]
        public void CheckLoad_A0i_ToA1()
        {
            // LEA <ea>, An
            // 0100 0011 1101 1000  0x43D8

            // LEA (A0)+, A1

            byte[] data = new byte[]
            {
                0x43, 0xD8
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x0, 0x00FF0000);
            state.WriteByte(0x00FF0000, 0xAA);
            state.FetchOpCode();

            var opcode = new LEA(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Long));

            Assert.That(state.PC, Is.EqualTo(0x00000002));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0004));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0xAA000000));
        }

        /// <summary>
        /// This should be invalid
        /// </summary>
        [Test]
        public void CheckLoad_dA0_ToA1()
        {
            // LEA <ea>, An
            // 0100 0011 1110 0000  0x43E0

            // LEA -(A0), A1

            byte[] data = new byte[]
            {
                0x43, 0xE0
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x0, 0x00FF0004);
            state.WriteByte(0x00FF0000, 0xAA);
            state.FetchOpCode();

            var opcode = new LEA(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Long));

            Assert.That(state.PC, Is.EqualTo(0x00000002));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0000));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0xAA000000));
        }

        [Test]
        public void CheckLoad_d16A0_ToA1()
        {
            // LEA <ea>, An
            // 0100 0011 1110 1000  0x43E8
            // 0000 0000 0000 0100  0x0004

            // LEA -(A0), A1

            byte[] data = new byte[]
            {
                0x43, 0xE8, 0x00, 0x04
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x0, 0x00FF0004);
            state.WriteByte(0x00FF0000, 0xAA);
            state.FetchOpCode();

            var opcode = new LEA(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Long));

            Assert.That(state.PC, Is.EqualTo(0x00000004));
            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x00FF0004));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00FF0008));
        }

        [Test]
        public void CheckLoad_d16PC_ToA5()
        {
            // LEA <ea>, An
            // 0100 1011 1111 1010  0x4BFA
            // 0000 0000 0111 1100  0x007C

            // LEA (d16, PC), A5
            // LEA (124, PC), A5

            byte[] data = new byte[]
            {
                0x4B, 0xFA, 0x00, 0x7C
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.FetchOpCode();

            var opcode = new LEA(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Long));

            Assert.That(state.PC, Is.EqualTo(0x00000004));
            Assert.That(state.ReadAReg(0x5), Is.EqualTo(0x7E));
        }
    }
}
