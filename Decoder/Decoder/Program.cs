using System.IO;

namespace Decoder
{
    class Program
    {
        static void Main(string[] args)
        {
            // Mega Drive ROMs have 256 byte header so we start decoding at 0x0200

            // https://wiki.megadrive.org/index.php?title=MD_Rom_Header

            //Header:
            //    .ascii  "SEGA MEGA DRIVE "                                      /* Console Name (16) */
            //    .ascii  "(C)SEGA 2012.MAR"                                      /* Copyright Information (16) */
            //    .ascii  "MY PROG                                         "      /* Domestic Name (48) */
            //    .ascii  "MY PROG                                         "      /* Overseas Name (48) */
            //    .ascii  "GM 00000000-00"                                        /* Serial Number (2, 14) */
            //    .word   0x0000                                                  /* Checksum (2) */
            //    .ascii  "JD              "                                      /* I/O Support (16) */
            //    .long   0x00000000                                              /* ROM Start Address (4) */
            //    .long   0x20000                                                 /* ROM End Address (4) */
            //    .long   0x00FF0000                                              /* Start of Backup RAM (4) */
            //    .long    0x00FFFFFF                                              /* End of Backup RAM (4) */
            //    .ascii  "                        "                              /* Modem Support (12) */
            //    .ascii  "                                        "              /* Memo (40) */
            //    .ascii  "JUE             "                                      /* Country Support (16) */

            // TODO: decode header data
            new RomDecoder(File.ReadAllBytes(@"C:\Users\robertb\Downloads\Flashback (Europe) (Rev A).md")).Decode(0x0200);
        }
    }
}
