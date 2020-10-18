using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using vtbulk.Objects;

namespace vtbulk
{
    public class Downloader
    {
        private readonly CommandLineArgumentsItem Arguments;

        public Downloader(CommandLineArgumentsItem arguments)
        {
            Arguments = arguments;
        }

        private void WriteLine(string line, Exception exception = null, bool verbose = true)
        {
            line = $"{DateTime.Now} - {line}";

            if (verbose && exception != null)
            {
                line = $"{line} (Exception: {exception})";
            }

            if (Arguments.VerboseOutput || verbose)
            {
                Console.WriteLine(line);
            }
        }

        public void Download(string[] hashes)
        {
            if (!hashes.Any())
            {
                Console.WriteLine("No hashes to download from the selected source");

                return;
            }

            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = (Arguments.EnableMultithreading ? Environment.ProcessorCount : 1)
            };

            if (Arguments.EnableMultithreading)
            {
                WriteLine($"Multi-Threading Enabled - {Environment.ProcessorCount} Threads will be used", verbose: arguments.VerboseOutput);
            }
            else
            {
                WriteLine("Multi-Threading Disabled - Single Threaded Mode will be used", verbose: arguments.VerboseOutput);
            }

            Parallel.ForEach(hashes, parallelOptions, async (hash, state) =>
            {
                var fullPath = Path.Combine(Arguments.OutputFilePath, hash);

                if (File.Exists(fullPath))
                {
                    WriteLine($"{fullPath} already exists - ignoring", null, Arguments.VerboseOutput);
                    return;
                }

                var response = await VTHTTPHandler.DownloadAsync(Arguments.VTKey, hash);

                switch (response.Status)
                {
                    case Enums.DownloadResponseStatus.UNEXPECTED_HTTP_ERROR:
                        WriteLine($"{hash} could not be downloaded due to an unexpected error", response.DownloadException);
                        break;
                    case Enums.DownloadResponseStatus.SAMPLE_NOT_FOUND:
                        WriteLine($"{hash} was not found in VirusTotal");
                        break;
                    case Enums.DownloadResponseStatus.INVALID_VT_KEY:
                        WriteLine("Invalid Virus Total Key - aborting operation");

                        state.Break();
                        return;
                    case Enums.DownloadResponseStatus.SUCCESS:
                        try
                        {
                            File.WriteAllBytes(fullPath, response.Data);

                            WriteLine($"{hash} was downloaded to {fullPath}");
                        }
                        catch (Exception ex)
                        {
                            WriteLine($"{hash} failed to write to disk properly", ex);
                        }
                        break;
                    case Enums.DownloadResponseStatus.CANNOT_WRITE_FILE:
                        break;
                    case Enums.DownloadResponseStatus.CANNOT_CONNECT_TO_VT:
                        WriteLine("Could not connect to VirusTotal - aborting");

                        state.Break();
                        break;
                }
            });
        }
    }
}