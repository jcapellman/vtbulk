using System;

using vtbulk.Enums;

namespace vtbulk.Objects
{
    public class DownloadResponseItem
    {
        public DownloadResponseStatus Status { get; set; }

        public Exception DownloadException { get; set; }

        public byte[] Data { get; set; }

        public DownloadResponseItem(byte[] data)
        {
            Status = DownloadResponseStatus.SUCCESS;
            Data = data;
        }

        public DownloadResponseItem(DownloadResponseStatus status, Exception exception = null)
        {
            Status = status;
            DownloadException = exception;
        }
    }
}