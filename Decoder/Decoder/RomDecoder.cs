using Decoder.OpCodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Decoder
{
    class RomDecoder
    {
        bool running = true;

        private Data data;

        private int programCounter;

        private ushort opcodeRegister;

        private Dictionary<int, OpCode> disassembly;

        public RomDecoder(Data data)
        {
            this.data = data;

            disassembly = new Dictionary<int, OpCode>();
        }

        public void Decode(int origin)
        {
            programCounter = origin;

            try
            {
                do
                {
                    Console.WriteLine("Addr\t0x{0:X4} ({0})", programCounter);

                    opcodeRegister = data.ReadWord(programCounter);

                    Console.WriteLine("OpCode\t0x{0:X4}", opcodeRegister);

                    Console.WriteLine(opcodeRegister.ToBinary());

                    OpCode opcode = new OpCodeDecoder().Decode(data, programCounter, opcodeRegister);

                    if (opcode is Invalid)
                    {
                        throw new Exception("invalid opcode");
                    }

                    disassembly.Add(programCounter, opcode);

                    programCounter += opcode.PCDisplacement;

                    displayOpCode(opcode);

                } while (running);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            StringBuilder builder = new StringBuilder();
            for (int addr = 0x0000; addr < 0xFFFF; addr++)
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
