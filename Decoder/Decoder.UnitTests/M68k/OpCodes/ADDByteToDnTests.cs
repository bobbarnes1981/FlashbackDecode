using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decoder.UnitTests.M68k.OpCodes
{
    [TestFixture]
    class ADDByteToDnTests
    {
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
