using System;

namespace Decoder
{
    public class ConsoleWriter
    {
        public void Write(string text)
        {
            this.Write(text, Console.ForegroundColor);
        }

        public void Write(string text, ConsoleColor color)
        {
            ConsoleColor orig = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = orig;
        }
    }
}
