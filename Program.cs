using vtbulk.Helpers;

namespace vtbulk
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = CommandLineParserHelper.Parse(args);

            var hashes = HashLoader.GetHashes(arguments.InputHashFile);

            new Downloader(arguments).Download(hashes);
        }
    }
}