using System;
using vtbulk.Helpers;

namespace vtbulk
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var arguments = CommandLineParserHelper.Parse(args);

                new Downloader(arguments).Download();
            } 
            catch (ArgumentOutOfRangeException) { }
            catch (ArgumentNullException) { }
        }
    }
}