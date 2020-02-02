namespace Decoder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Decoder.M68k;
    using Decoder.M68k.OpCodes;

    /// <summary>
    /// Megadrive Decoder
    /// </summary>
    public class MegadriveDecoder
    {
        private bool running = true;

        private MegadriveState state;

        private Dictionary<uint, OpCode> disassembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="MegadriveDecoder"/> class.
        /// </summary>
        /// <param name="rom">rom data.</param>
        public MegadriveDecoder(Data rom)
        {
            RomHeader h = new RomHeader(rom);

            this.DisplayHeader(h);

            ushort checksum = rom.Checksum(0x200);
            string match = checksum == h.Checksum ? "match" : "invalid";

            Console.WriteLine($"Calc Checksum:\t${checksum}\t{match}");
            Console.WriteLine();

            this.state = new MegadriveState(rom, h.Origin, h.SP, h.RomStart, h.RomEnd, h.RamStart, h.RamEnd);

            this.disassembly = new Dictionary<uint, OpCode>();
        }

        public void Decode()
        {
            OpCodeDecoder decoder = new OpCodeDecoder();

            this.DisplayState(this.state);

            do
            {
                //Console.WriteLine("Enter to execute next instruction...");
                //Console.ReadLine();

                Console.WriteLine($"Addr\t${this.state.PC:X4} (#{this.state.PC})");

                this.state.FetchOpCode();

                Console.WriteLine($"OpCode\t${this.state.OpCode:X4}");

                Console.WriteLine($"%{this.state.OpCode.ToBinary()}");

                OpCode opcode = decoder.Decode(this.state);

                if (opcode is Invalid)
                {
                    throw new Exception("invalid opcode");
                }

                if (!this.disassembly.ContainsKey(opcode.Address))
                {
                    this.disassembly.Add(opcode.Address, opcode);
                }

                this.DisplayOpCode(opcode);

                //this.DisplayState(this.state);

            } while (this.running);

            StringBuilder builder = new StringBuilder();
            for (uint addr = 0x0000; addr < 0xFFFF; addr++)
            {
                builder.Append($"${addr:X4}");
                if (this.disassembly.ContainsKey(addr))
                {
                    builder.Append($"\t{this.disassembly[addr].Assembly}");
                }

                builder.AppendLine();
            }

            File.WriteAllText("output.asm", builder.ToString());
        }

        private void DisplayHeader(RomHeader header)
        {
            Console.WriteLine($"Console:\t{header.ConsoleName}");
            Console.WriteLine($"Copyright:\t{header.Copyright}");
            Console.WriteLine($"Name:\t\t{header.DomesticName}");
            Console.WriteLine($"Int Name:\t{header.InternationalName}");
            Console.WriteLine($"Version:\t{header.Version}");
            Console.WriteLine($"Checksum:\t${header.Checksum}");

            Console.WriteLine();
        }

        private void DisplayState(MegadriveState state)
        {
            Console.WriteLine($"A0=${state.ReadAReg(0x0):X8} A1=${state.ReadAReg(0x1):X8}");
            Console.WriteLine($"A2=${state.ReadAReg(0x2):X8} A3=${state.ReadAReg(0x3):X8}");
            Console.WriteLine($"A4=${state.ReadAReg(0x4):X8} A5=${state.ReadAReg(0x5):X8}");
            Console.WriteLine($"A6=${state.ReadAReg(0x6):X8} A7=${state.ReadAReg(0x7):X8}");
            Console.WriteLine($"D0=${state.ReadDReg(0x0):X8} D1=${state.ReadDReg(0x1):X8}");
            Console.WriteLine($"D2=${state.ReadDReg(0x2):X8} D3=${state.ReadDReg(0x3):X8}");
            Console.WriteLine($"D4=${state.ReadDReg(0x4):X8} D5=${state.ReadDReg(0x5):X8}");
            Console.WriteLine($"D6=${state.ReadDReg(0x6):X8} D7=${state.ReadDReg(0x7):X8}");
            Console.WriteLine($"PC=${state.PC:X8} SR=${state.SR:X8}");

            Console.WriteLine();
        }

        private void DisplayOpCode(OpCode opcode)
        {
            Console.WriteLine(opcode.Syntax);
            Console.WriteLine(opcode.Description);
            Console.WriteLine(opcode.Operation);
            Console.WriteLine(opcode.Assembly);

            Console.WriteLine();
        }
    }
}
