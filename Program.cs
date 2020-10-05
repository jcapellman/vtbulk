using System;
using System.IO;
using System.Threading.Tasks;

using vtbulk.Helpers;

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

        static void Main(string[] args)
        {
            var arguments = CommandLineParserHelper.Parse(args);

            var hashes = GetHashes(arguments.InputHashFile);

            Parallel.ForEach(hashes, async (hash, state) =>
            {
                if (File.Exists(hash))
                {
                    return;
                }

                var response = await VTHTTPHandler.DownloadAsync(arguments.VTKey, hash);

                switch (response.Status)
                {
                    case Enums.DownloadResponseStatus.SAMPLE_NOT_FOUND:
                        Console.WriteLine($"{hash} was not found in VirusTotal");
                        break;
                    case Enums.DownloadResponseStatus.INVALID_VT_KEY:
                        Console.WriteLine("Invalid Virus Total Key - aborting operation");

                        state.Break();
                        return;
                    case Enums.DownloadResponseStatus.SUCCESS:
                        var fullPath = Path.Combine(arguments.OutputFilePath, hash);
                        File.WriteAllBytes(fullPath, response.Data);

                        Console.WriteLine($"{hash} was downloaded to {fullPath}");
                        break;
                }
            });
        }
    }
}