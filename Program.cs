using vtbulk.Helpers;

namespace vtbulk
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = CommandLineParserHelper.Parse(args);

            new Downloader(arguments).Download();
        }
    }
}