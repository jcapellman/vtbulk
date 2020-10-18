using System;

using vtbulk.Enums;

namespace vtbulk.Objects
{
    public class DownloadResponseItem
    {
        public DownloadResponseStatus Status { get; }

        public Exception DownloadException { get; }

        public byte[] Data { get; }

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