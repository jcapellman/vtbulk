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

        private const string VTKey = "";

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough arguments: <path to hashes> <virus total key>");

                return;
            }

            var hashes = GetHashes(args[0]);

            var handler = new VTHTTPHandler();

            Parallel.ForEach(hashes, hash =>
            {
                if (File.Exists(hash))
                {
                    return;
                }

                var response = VTHTTPHandler.DownloadAsync(args[1], hash).Result;

                if (response.Status == Enums.DownloadResponseStatus.SUCCESS)
                {
                    File.WriteAllBytes(hash, response.Data);
                }
            });
        }
    }
}