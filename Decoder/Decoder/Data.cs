namespace Decoder
{
    using System.IO;

    public class Data
    {
        private byte[] data;

        public Data(string path)
            : this(File.ReadAllBytes(path))
        {
        }

        public Data(byte[] data)
        {
            this.data = data;
        }

        public ushort Checksum(uint address)
        {
            ulong sum = 0;
            for (uint i = address; i < data.Length; i += 2)
            {
                sum += this.ReadWord(i);
            }

            return (ushort)sum;
        }

        /// <summary>
        /// Read 1 byte.
        /// </summary>
        /// <returns></returns>
        public byte ReadByte(uint address)
        {
            // big endian ordering of bytes in memory (https://en.wikipedia.org/wiki/Endianness)
            // 0x1234 stored as 12 34

            byte b = 0x00;

            b |= (byte)(data[address + 0] << 0);

            return b;
        }

        /// <summary>
        /// Read 2 bytes.
        /// </summary>
        /// <returns></returns>
        public ushort ReadWord(uint address)
        {
            // big endian ordering of bytes in memory (https://en.wikipedia.org/wiki/Endianness)
            // 0x1234 stored as 12 34

            ushort w = 0x00;

            w |= (ushort)(data[address + 0] << 8);
            w |= (ushort)(data[address + 1] << 0);

            return w;
        }

        /// <summary>
        /// Read 4 bytes.
        /// </summary>
        /// <returns></returns>
        public uint ReadLong(uint address)
        {
            // big endian ordering of bytes in memory (https://en.wikipedia.org/wiki/Endianness)
            // 0x1234 stored as 12 34

            uint l = 0x00;

            l |= (uint)(this.data[address + 0] << 24);
            l |= (uint)(this.data[address + 1] << 16);
            l |= (uint)(this.data[address + 2] << 8);
            l |= (uint)(this.data[address + 3] << 0);

            return l;
        }

        public void WriteByte(uint address, byte b)
        {
            System.Console.WriteLine($"{address:X8} {b:X2}");
            this.data[address + 0] = b;
        }
    }
}
