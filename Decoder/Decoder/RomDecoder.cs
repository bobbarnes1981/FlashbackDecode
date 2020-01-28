using Decoder.OpCodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Decoder
{
    class RomDecoder
    {
        private bool running = true;

        private MachineState state;

        private Dictionary<uint, OpCode> disassembly;

        public RomDecoder(Data data)
        {
            state = new MachineState(data);

            disassembly = new Dictionary<uint, OpCode>();
        }

        public void Decode()
        {
            do
            {
                Console.WriteLine("Addr\t0x{0:X4} ({0})", state.PC);

                state.FetchOpCode();

                Console.WriteLine("OpCode\t0x{0:X4}", state.OpCode);

                Console.WriteLine(state.OpCode.ToBinary());

                OpCode opcode = new OpCodeDecoder().Decode(state);

                if (opcode is Invalid)
                {
                    throw new Exception("invalid opcode");
                }

                disassembly.Add(opcode.Address, opcode);

                displayOpCode(opcode);

                Console.WriteLine("Enter to execute next instruction...");
                Console.ReadLine();

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

        private void displayOpCode(OpCode opcode)
        {
            Console.WriteLine(opcode.FullName);
            Console.WriteLine(opcode.Description);

            Console.WriteLine(opcode.Operation);
            Console.WriteLine(opcode.Syntax);
            Console.WriteLine(opcode.Assembly);

            Console.WriteLine();
        }
    }
}
