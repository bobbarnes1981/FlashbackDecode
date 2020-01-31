namespace Decoder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Decoder.M68k;
    using Decoder.M68k.OpCodes;

    class MegadriveDecoder
    {
        private bool running = true;

        private MegadriveState state;

        private Dictionary<uint, OpCode> disassembly;

        public MegadriveDecoder(Data rom)
        {
            RomHeader h = new RomHeader(rom);

            displayHeader(h);

            ushort checksum = rom.Checksum(0x200);
            string match = checksum == h.Checksum ? "match" : "invalid";

            Console.WriteLine($"Calc Checksum:\t{checksum}\t{match}");
            Console.WriteLine();

            state = new MegadriveState(rom, h.Origin, h.SP, h.RomStart, h.RomEnd, h.RamStart, h.RamEnd);

            disassembly = new Dictionary<uint, OpCode>();
        }

        public void Decode()
        {
            OpCodeDecoder decoder = new OpCodeDecoder();

            displayState(state);

            do
            {
                Console.WriteLine("Enter to execute next instruction...");
                Console.ReadLine();

                Console.WriteLine("Addr\t0x{0:X4} ({0})", state.PC);

                state.FetchOpCode();

                Console.WriteLine("OpCode\t0x{0:X4}", state.OpCode);

                Console.WriteLine(state.OpCode.ToBinary());

                OpCode opcode = decoder.Decode(state);

                if (opcode is Invalid)
                {
                    throw new Exception("invalid opcode");
                }

                disassembly.Add(opcode.Address, opcode);

                displayOpCode(opcode);

                displayState(state);

            } while (running);

            StringBuilder builder = new StringBuilder();
            for (uint addr = 0x0000; addr < 0xFFFF; addr++)
            {
                builder.AppendFormat("0x{0:X4}", addr);
                if (disassembly.ContainsKey(addr))
                {
                    builder.AppendFormat("\t{0}", disassembly[addr].Assembly);
                }

                builder.AppendLine();
            }

            File.WriteAllText("output.asm", builder.ToString());
        }

        private void displayHeader(RomHeader header)
        {
            Console.WriteLine($"Console:\t{header.ConsoleName}");
            Console.WriteLine($"Copyright:\t{header.Copyright}");
            Console.WriteLine($"Name:\t\t{header.DomesticName}");
            Console.WriteLine($"Int Name:\t{header.InternationalName}");
            Console.WriteLine($"Version:\t{header.Version}");
            Console.WriteLine($"Checksum:\t{header.Checksum}");

            Console.WriteLine();
        }

        private void displayState(MegadriveState state)
        {
            Console.WriteLine($"A0=0x{state.ReadAReg(0x0):X8} A1=0x{state.ReadAReg(0x1):X8}");
            Console.WriteLine($"A2=0x{state.ReadAReg(0x2):X8} A3=0x{state.ReadAReg(0x3):X8}");
            Console.WriteLine($"A4=0x{state.ReadAReg(0x4):X8} A5=0x{state.ReadAReg(0x5):X8}");
            Console.WriteLine($"A6=0x{state.ReadAReg(0x6):X8} A7=0x{state.ReadAReg(0x7):X8}");
            Console.WriteLine($"D0=0x{state.ReadDReg(0x0):X8} D1=0x{state.ReadDReg(0x1):X8}");
            Console.WriteLine($"D2=0x{state.ReadDReg(0x2):X8} D3=0x{state.ReadDReg(0x3):X8}");
            Console.WriteLine($"D4=0x{state.ReadDReg(0x4):X8} D5=0x{state.ReadDReg(0x5):X8}");
            Console.WriteLine($"D6=0x{state.ReadDReg(0x6):X8} D7=0x{state.ReadDReg(0x7):X8}");
            Console.WriteLine($"PC=0x{state.PC:X8} SR=0x{state.SR:X8}");

            Console.WriteLine();
        }

        private void displayOpCode(OpCode opcode)
        {
            Console.WriteLine(opcode.Syntax);
            Console.WriteLine(opcode.Description);
            Console.WriteLine(opcode.Operation);
            Console.WriteLine(opcode.Assembly);

            Console.WriteLine();
        }
    }
}
