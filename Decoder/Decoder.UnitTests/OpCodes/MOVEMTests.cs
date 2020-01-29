﻿using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.OpCodes
{
    [TestFixture]
    class MOVEMTests
    {
        [Test]
        public void CheckA5PostIncMoveWordToReg()
        {
            // MOVEM <ea>, <ea>
            // 0100 1100 1001 1101  0x4C9D
            // 0000 0000 1110 0000  0x00E0

            // MOVEM (A5+), 0000 0000 1110 0000

            byte[] data = new byte[]
            {
                0x4C, 0x9D, 0x00, 0xE0
            };

            MachineState state = new MachineState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteAReg(0x5, 0xFF0000);
            for (int i = 0; i < 16; i++)
            {
                state.WriteByte((uint)(0xFF0000 + i), (byte)(0x00 + i));
            }
            state.FetchOpCode();

            var opcode = new MOVEM(state);

            Assert.That(opcode.Size, Is.EqualTo(Size.Word));


            Assert.That(state.ReadAReg(0x0), Is.EqualTo(0x0000));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x0000));
            Assert.That(state.ReadAReg(0x2), Is.EqualTo(0x0000));
            Assert.That(state.ReadAReg(0x3), Is.EqualTo(0x0000));
            Assert.That(state.ReadAReg(0x4), Is.EqualTo(0x0000));
            Assert.That(state.ReadAReg(0x5), Is.EqualTo(0xFF0020));
            Assert.That(state.ReadAReg(0x6), Is.EqualTo(0x0000));
            Assert.That(state.ReadAReg(0x7), Is.EqualTo(0x0000));

            Assert.That(state.ReadDReg(0x0), Is.EqualTo(0x0000));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x0000));
            Assert.That(state.ReadDReg(0x2), Is.EqualTo(0x0000));
            Assert.That(state.ReadDReg(0x3), Is.EqualTo(0x0000));
            Assert.That(state.ReadDReg(0x4), Is.EqualTo(0x0000));
            Assert.That(state.ReadDReg(0x5), Is.EqualTo(0x0A0B));
            Assert.That(state.ReadDReg(0x6), Is.EqualTo(0x0C0D));
            Assert.That(state.ReadDReg(0x7), Is.EqualTo(0x0E0F));

            Assert.That(state.PC, Is.EqualTo(0x00000004));
            Assert.That(state.ReadAReg(0x5), Is.EqualTo(0x00FF0020));
        }
    }
}
