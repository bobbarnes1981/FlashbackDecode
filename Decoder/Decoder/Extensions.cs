using System.Text;

namespace Decoder
{
    static class Extensions
    {
        public static string ToBinary(this ushort s)
        {
            StringBuilder builder = new StringBuilder();
            for (ushort u = 32768; u > 0; u = (ushort)(u / 2))
            {
                builder.AppendFormat("{0}", (s & u) == u ? 1 : 0);
            }

            return builder.ToString();
        }

        public static ushort GetBits(this ushort s, int offset, int length)
        {
            // TODO: validate values
            ushort lengthMask = 0x0000;
            ushort count = 1;
            for (int i = 1; i <= length; i++)
            {
                lengthMask |= count;
                count += count;
            }

            return (ushort)((s >> offset) & lengthMask);
        }
    }
}
