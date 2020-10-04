using System;
using System.IO;
using System.Threading.Tasks;

namespace vtbulk
{
    class Program
    {
        private static string[] GetHashes(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"HASH File does not exist: {fileName}");
            }

            return File.ReadAllLines(fileName);
        }

        private static (string HashFile, string VTKey, string OutputPath) ParseArguments(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough arguments: <path to hashes> <virus total key>");

                throw new ArgumentOutOfRangeException(nameof(args));
            }

            switch (args.Length)
            {
                case 3:
                    return (args[0], args[1], args[2]);
                default:
                    break;
            }

            return (args[0], args[1], AppContext.BaseDirectory);
        }
        static void Main(string[] args)
        {
            var arguments = ParseArguments(args);

            var hashes = GetHashes(arguments.HashFile);

            var handler = new VTHTTPHandler();

            Parallel.ForEach(hashes, hash =>
            {
                if (File.Exists(hash))
                {
                    return;
                }

                var response = VTHTTPHandler.DownloadAsync(arguments.VTKey, hash).Result;

                if (response.Status == Enums.DownloadResponseStatus.SUCCESS)
                {
                    File.WriteAllBytes(Path.Combine(arguments.OutputPath, hash), response.Data);
                }
            });
        }
    }
}