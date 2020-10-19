using System;
using System.Linq;
using System.Reflection;
using vtbulk.Interfaces;
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

            var hashSources = Assembly.GetExecutingAssembly().GetTypes()
                .Where(a => a.BaseType == typeof(IHashList) && !a.IsInterface)
                .Select(b => (IHashList) Activator.CreateInstance(b)).ToList();

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
                    case "inputsource":
                        item.HashSource = hashSources.FirstOrDefault(a => string.Equals(a.Name, optionValue, StringComparison.CurrentCultureIgnoreCase));

                        if (item.HashSource == null)
                        {
                            Console.WriteLine($"{optionValue} is an invalid inputsource option. Supported options are: {string.Join(',', hashSources.Select(a => a.Name))}");
                        }

                        break;
                    case "inputsourceargument":
                        item.HashListSourceArgument = optionValue;
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
                    case "EnableMultithreading":
                        try
                        {
                            item.EnableMultithreading = Convert.ToBoolean(optionValue);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine($"Invalid value for enablemultithreading flag ({optionValue})");
                        }
                        break;
                    default:
                        Console.WriteLine($"Invalid option ({option})");
                        break;
                }
            }

            if (item.VTKey is null || item.HashSource == null) {
                Console.WriteLine("inputsource and vtkey are required");

                throw new ArgumentOutOfRangeException("inputsource and vtkey are required");
            }

            if (item.HashSource.ArgumentRequired && string.IsNullOrEmpty(item.HashListSourceArgument))
            {
                Console.WriteLine($"Input Source of type ({item.HashSource.Name}) requires an argument");

                throw new System.ArgumentOutOfRangeException(nameof(args), "inputsourceargument is required for the selected input source");
            }

            return item;
        }
    }
}