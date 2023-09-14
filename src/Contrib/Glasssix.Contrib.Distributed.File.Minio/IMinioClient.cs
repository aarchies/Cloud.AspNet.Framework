using Minio;
using Minio.DataModel;
using System.IO;
using System.Threading.Tasks;

namespace Glasssix.Contrib.File.Storage
{
    public interface IMinioClient
    {

        MinioClient MinioClient { get; }


        /// <summary>
        /// 文件授权
        /// </summary>
        /// <param name="fromObjectName"></param>
        /// <param name="type"></param>
        /// <param name="sseSrc"></param>
        /// <param name="sseDest"></param>
        /// <returns></returns>
        Task<string> FileAuthorized(string fromObjectName, string type, IServerSideEncryption sseSrc, IServerSideEncryption sseDest);


        /// <summary>
        /// 设置策略
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        Task SetPolicy(string json);


        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bucketName">可选桶</param>
        /// <returns></returns>
        Task GetObjectAsync(string path, string fileName, string? bucketName = null);


        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<MemoryStream> GetStreamAsync(string fileName, string type);


        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<MemoryStream?> GetStreamAsync(string url);


        /// <summary>
        /// 异步上传
        /// </summary>
        /// <param name="name">文件名称/文件路径及名称(须含拓展名)</param>
        /// <param name="array"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<string> UploadAsync(string name, byte[] array, string? bucketName = null);
    }
}
