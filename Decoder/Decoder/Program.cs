namespace Decoder
{
    class Program
    {
        static void Main(string[] args)
        {
            new MegadriveDecoder(new Data(@"ROM\Flashback (Europe) (Rev A).md")).Decode();
        }
    }
}
