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

                var optionValue = args[x + 1];

                switch (option)
                {
                    case "vtkey":
                        item.VTKey = optionValue;
                        break;
                    case "inputfile":
                        item.InputHashFile = optionValue;
                        break;
                    case "outputpath":
                        item.OutputFilePath = optionValue;
                        break;
                    case "verbose":
                        try
                        {
                            item.VerboseOutput = Convert.ToBoolean(optionValue);
                        } catch (FormatException)
                        {
                            Console.WriteLine($"Invalid value for verbose flag ({optionValue})");
                        }

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