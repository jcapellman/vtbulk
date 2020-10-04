using System;

using vtbulk.Objects;

namespace vtbulk.Helpers
{
    public class CommandLineParserHelper
    {
        public static CommandLineArgumentsItem Parse(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough arguments: <path to hashes> <virus total key>");

                throw new ArgumentOutOfRangeException(nameof(args));
            }

            var item = new CommandLineArgumentsItem
            {
                InputHashFile = args[0],
                VTKey = args[1]
            };

            switch (args.Length)
            {
                case 3:
                    item.OutputFilePath = args[2];
                    break;
                default:
                    item.OutputFilePath = AppContext.BaseDirectory;
                    break;
            }

            return item;
        }
    }
}