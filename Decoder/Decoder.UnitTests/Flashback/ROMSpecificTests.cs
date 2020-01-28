using Decoder.OpCodes;
using NUnit.Framework;

namespace Decoder.UnitTests.Flashback
{
    [TestFixture]
    public class ROMSpecificTests
    {
        //[Test]
        //public void TEST_0x0200()
        //{
        //    // 0011000111111100     0x31FC
        //    // 0000000000010111     0x0017
        //    // 1111100000011010     0xF416

        //    // move #23 to 0xF416

        //    byte[] data = new byte[]
        //    {
        //        0x31, 0xFC, 0x00, 0x17, 0xF4, 0x16
        //    };

        //    MachineState state = new MachineState(new Data(data), 0x0000);
        //    state.FetchOpCode();

        //    var opcode = new MOVE(state);

        //    byte actualData = (byte)state.Read(0xF416);

        //    Assert.That(actualData, Is.EqualTo(23));
        //    Assert.That(state.PC, Is.EqualTo(0x6));
        //}

        //[Test]
        //public void TEST_0x0206()
        //{
        //    // 0110000000000000     0x6000
        //    // 0000000101100110     0x0166

        //    // Branch Always (W) 358 (+2)

        //    byte[] data = new byte[]
        //    {
        //        0x60, 0x00, 0x01, 0x66
        //    };

        //    MachineState state = new MachineState(new Data(data), 0x0000);
        //    state.FetchOpCode();

        //    var opcode = new BRA(state);

        //    Assert.That(state.PC, Is.EqualTo(360));
        //}

        //[Test]
        //public void TEST_0x036E()
        //{
        //    // 0100011011111100     0x46FC
        //    // 0010011100000000     0x2700

        //    byte[] data = new byte[]
        //    {
        //        0x46, 0xFC, 0x27, 0x00
        //    };

        //    MachineState state = new MachineState(new Data(data), 0x0000);
        //    state.FetchOpCode();

        //    var opcode = new MOVEtoSR(state);

        //    Assert.That(state.PC, Is.EqualTo(0x0004));
        //    Assert.That(state.SR, Is.EqualTo(0x2700));
        //}

        //[Test]
        //public void TEST_0x0372()
        //{
        //    // 0100100011111000     0x48F8
        //    // 1111111111111111     0xFFFF
        //    // 1111010000011100     0xF41C

        //    // move all registers to memory 0xF41C

        //    byte[] data = new byte[]
        //    {
        //        0x48, 0xF8, 0xFF, 0xFF, 0xF4, 0x1C
        //    };

        //    MachineState state = new MachineState(new Data(data), 0x0000);
        //    state.FetchOpCode();

        //    state.WriteAReg(0x0, 0x1);
        //    state.WriteAReg(0x1, 0x2);
        //    state.WriteAReg(0x2, 0x3);
        //    state.WriteAReg(0x3, 0x4);
        //    state.WriteAReg(0x4, 0x5);
        //    state.WriteAReg(0x5, 0x6);
        //    state.WriteAReg(0x6, 0x7);

        //    state.WriteDReg(0x0, 0x1);
        //    state.WriteDReg(0x1, 0x2);
        //    state.WriteDReg(0x2, 0x3);
        //    state.WriteDReg(0x3, 0x4);
        //    state.WriteDReg(0x4, 0x5);
        //    state.WriteDReg(0x5, 0x6);
        //    state.WriteDReg(0x6, 0x7);

        //    var opcode = new MOVEM(state);

        //    Assert.That(state.PC, Is.EqualTo(0x0006));

        //    Assert.Fail("not implemented");
        //}

        //[Test]
        //public void TEST_0x0378()
        //{
        //    // 0100000111111001     0x41F9
        //    // 0000000011000000     0x00C0
        //    // 0000000000000000     0x0000

        //    // Load 0x00C00000 to A0

        //    byte[] data = new byte[]
        //    {
        //        0x41, 0xF9, 0x00, 0xC0, 0x00, 0x00
        //    };

        //    MachineState state = new MachineState(new Data(data), 0x0000);
        //    state.FetchOpCode();

        //    var opcode = new LEA(state);

        //    var actualValue = state.ReadAReg(0x0);

        //    Assert.That(state.PC, Is.EqualTo(0x0006));
        //    Assert.That(actualValue, Is.EqualTo(0x00C00000));
        //}
    }
}
