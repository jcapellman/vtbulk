using System.Net.Http;
using System.Threading.Tasks;

using vtbulk.Enums;
using vtbulk.Objects;

namespace vtbulk
{
    public class VTHTTPHandler
    {
        public static async Task<DownloadResponseItem> DownloadAsync(string vtKey, string hash)
        {
            try
            {
                using var httpClient = new HttpClient();

                var file = await httpClient.GetByteArrayAsync(
                    $"https://www.virustotal.com/vtapi/v2/file/download?apikey={vtKey}&hash={hash}");

                if (file == null)
                {
                    return new DownloadResponseItem(DownloadResponseStatus.CANNOT_CONNECT_TO_VT);
                }

                return new DownloadResponseItem(file);
            }
            catch (HttpRequestException requestException)
            {
                return requestException.StatusCode switch
                {
                    System.Net.HttpStatusCode.Forbidden => new DownloadResponseItem(DownloadResponseStatus.INVALID_VT_KEY),
                    System.Net.HttpStatusCode.NotFound => new DownloadResponseItem(DownloadResponseStatus.SAMPLE_NOT_FOUND),
                    _ => new DownloadResponseItem(DownloadResponseStatus.UNEXPECTED_HTTP_ERROR, requestException),
                };
            }
        }
    }
}