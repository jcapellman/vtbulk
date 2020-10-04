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
                using (var httpClient = new HttpClient())
                {
                    var file = await httpClient.GetByteArrayAsync(
                        $"https://www.virustotal.com/vtapi/v2/file/download?apikey={vtKey}&hash={hash}");

                    if (file == null)
                    {
                        return new DownloadResponseItem(DownloadResponseStatus.CANNOT_CONNECT_TO_VT);
                    }

                    return new DownloadResponseItem(file);
                }
            }
            catch (HttpRequestException requestException)
            {
                switch (requestException.StatusCode)
                {
                    case System.Net.HttpStatusCode.Forbidden:
                        return new DownloadResponseItem(DownloadResponseStatus.INVALID_VT_KEY);
                    case System.Net.HttpStatusCode.NotFound:
                        return new DownloadResponseItem(DownloadResponseStatus.SAMPLE_NOT_FOUND);
                    default:
                        return new DownloadResponseItem(DownloadResponseStatus.UNEXPECTED_HTTP_ERROR, requestException);
                }
            }
        }
    }
}