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

        private static void WriteLine(string line, bool verbose = true)
        {
            if (verbose)
            {
                Console.WriteLine($"{DateTime.Now} - {line}");
            }
        }

        static void Main(string[] args)
        {
            var arguments = CommandLineParserHelper.Parse(args);

            var hashes = GetHashes(arguments.InputHashFile);

            Parallel.ForEach(hashes, async (hash, state) =>
            {
                var fullPath = Path.Combine(arguments.OutputFilePath, hash);

                if (File.Exists(fullPath))
                {
                    WriteLine($"{fullPath} already exists - ignoring", arguments.VerboseOutput);
                    return;
                }

                var response = await VTHTTPHandler.DownloadAsync(arguments.VTKey, hash);

                switch (response.Status)
                {
                    case Enums.DownloadResponseStatus.UNEXPECTED_HTTP_ERROR:
                        WriteLine($"{hash} could not be downloaded due to an unexpected error: {response.DownloadException}");
                        break;
                    case Enums.DownloadResponseStatus.SAMPLE_NOT_FOUND:
                        WriteLine($"{hash} was not found in VirusTotal");
                        break;
                    case Enums.DownloadResponseStatus.INVALID_VT_KEY:
                        WriteLine("Invalid Virus Total Key - aborting operation");

                        state.Break();
                        return;
                    case Enums.DownloadResponseStatus.SUCCESS:
                        File.WriteAllBytes(fullPath, response.Data);

                        WriteLine($"{hash} was downloaded to {fullPath}");
                        break;
                }
            });
        }
    }
}