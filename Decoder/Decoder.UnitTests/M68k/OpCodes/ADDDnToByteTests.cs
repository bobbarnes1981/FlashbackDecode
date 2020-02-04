using Decoder.M68k.OpCodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decoder.UnitTests.M68k.OpCodes
{
    [TestFixture]
    class ADDDnToByteTests
    {
        [Test(Description = "ADD D0,D1")]
        public void CheckAddD0ToD1()
        {
            // ADD Dn,<ea>
            // 1101 dddD ssmm mxxx
            // 1101 0001 0000 0001  0xD101
            // ADD D0,D1

            byte[] data = new byte[]
            {
                0xD1, 0x01
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x0, 0x22221111);
            state.WriteDReg(0x1, 0x33332222);
            state.FetchOpCode();

            //throw exception
            var opcode = new ADD(state);

            Assert.That(opcode.Assembly, Is.EqualTo("ADD D0,D1"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadDReg(0x1), Is.EqualTo(0x00005678));
        }

        [Test(Description = "ADD D0,A1")]
        public void CheckAddD0ToA1()
        {
            // ADD Dn,<ea>
            // 1101 dddD ssmm mxxx
            // 1101 0001 0000 1001  0xD109
            // ADD D0,D1

            byte[] data = new byte[]
            {
                0xD1, 0x09
            };

            MegadriveState state = new MegadriveState(new Data(data), 0x00000000, 0x00000000, 0x000000, 0x3FFFFF, 0x0FF0000, 0xFFFFFF);
            state.WriteDReg(0x0, 0x22221111);
            state.WriteAReg(0x1, 0x33332222);
            state.FetchOpCode();

            //throw exception
            var opcode = new ADD(state);

            Assert.That(opcode.Assembly, Is.EqualTo("ADD D0,A1"));
            Assert.That(state.PC, Is.EqualTo(0x02));
            Assert.That(state.ReadAReg(0x1), Is.EqualTo(0x00005678));
        }

        // to
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
