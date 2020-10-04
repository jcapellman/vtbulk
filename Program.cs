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
                    File.WriteAllBytes(Path.Combine(arguments.OutputFilePath, hash), response.Data);
                }
            });
        }
    }
}