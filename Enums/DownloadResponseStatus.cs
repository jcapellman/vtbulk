namespace vtbulk.Enums
{
    public enum DownloadResponseStatus
    {
        INVALID_VT_KEY,
        SUCCESS,
        CANNOT_WRITE_FILE,
        CANNOT_CONNECT_TO_VT,
        UNEXPECTED_HTTP_ERROR,
        SAMPLE_NOT_FOUND
    }
}