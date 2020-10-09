using System;

using vtbulk.Objects;

namespace vtbulk.Helpers
{
    public class CommandLineParserHelper
    {
        /// <exception cref="System.ArgumentNullException">Null arguments</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Invalid number of options</exception>
        public static CommandLineArgumentsItem Parse(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Length < 4)
            {
                Console.WriteLine("Not enough arguments: -path <path to hashes> -vtkey <virus total key>");

                throw new ArgumentOutOfRangeException(nameof(args));
            }

            if (args.Length % 2 != 0)
            {
                Console.WriteLine("Invalid number of options");

                throw new ArgumentOutOfRangeException("Invalid number of options");
            }

            var item = new CommandLineArgumentsItem();

            for (var x = 0; x < args.Length; x+=2)
            {
                var option = args[x].ToLower();

                switch (option)
                {
                    case "vtkey":
                        item.VTKey = args[x + 1];
                        break;
                    case "inputfile":
                        item.InputHashFile = args[x + 1];
                        break;
                    case "outputpath":
                        item.OutputFilePath = args[x + 1];
                        break;
                    case "verbose":
                        item.VerboseOutput = args[x + 1];
                        break;
                    default:
                        Console.WriteLine($"Invalid option ({option})");
                            break;
                }
            }

            if (item.VTKey is null || item.InputHashFile is null) {
                Console.WriteLine("inputfile and vtkey are required");

                throw new ArgumentOutOfRangeException("inputfile and vtkey are required");
            }

            return item;
        }
    }
}