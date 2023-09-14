namespace Glasssix.Contrib.File.Storage.Extensions.Dto
{
    public class UploadFileStream
    {
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[]? FileBytes { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// 文件长度
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string? FileType { get; set; }
    }
}