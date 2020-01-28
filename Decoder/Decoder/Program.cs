namespace Decoder
{
    class Program
    {
        static void Main(string[] args)
        {
            new RomDecoder(new Data(@"ROM\Flashback (Europe) (Rev A).md")).Decode();
        }
    }
}
