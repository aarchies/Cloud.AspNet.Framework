using Glasssix.Contrib.File.Storage.Extensions;
using Minio;
using Minio.DataModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Glasssix.Contrib.File.Storage
{
    public class MinioClients : IMinioClient
    {
        private readonly MinioOptions _connections;
        private readonly string _bucketName;

        public MinioClient MinioClient => GetMinioClient(_connections);

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="clientMax">最大连接池</param>
        public MinioClients(MinioOptions option)
        {
            _connections = option;
            _bucketName = option.BucketName!;
        }


        public async Task<string> FileAuthorized(string fromObjectName, string type, IServerSideEncryption sseSrc, IServerSideEncryption sseDest)
        {
            using var client = GetMinioClient(_connections);

            var cpSrcArgs = new CopySourceObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fromObjectName + type)
                .WithServerSideEncryption(sseSrc);
            var args = new CopyObjectArgs()
                .WithBucket(_bucketName)
                .WithObject("Authorized/" + fromObjectName + type)
                .WithCopyObjectSource(cpSrcArgs)
                .WithServerSideEncryption(sseDest);
            await client.CopyObjectAsync(args);
            RemoveObjectArgs rargs = new RemoveObjectArgs()
                           .WithBucket(_bucketName)
                           .WithObject(fromObjectName + type);
            await client.RemoveObjectAsync(rargs);

            return string.Concat("http:" + _connections.Connection.FirstOrDefault()!.Endpoint + "/", _bucketName, "/Authorized/", fromObjectName + type);
        }


        public async Task SetPolicy(string json)
        {
            using var client = GetMinioClient(_connections);
            var policyJson = $@"{{""Version"":""2012-10-17"",""Statement"":[{{""Action"":[""s3:ListBucket"",""s3:ListBucketMultipartUploads"",""s3:GetBucketLocation""],""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Resource"":[""arn:aws:s3:::{_bucketName}""],""Sid"":""""}},{{""Action"":[""s3:PutObject"",""s3:AbortMultipartUpload"",""s3:DeleteObject"",""s3:ListMultipartUploadParts""],""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Resource"":[""arn:aws:s3:::{_bucketName}/*""],""Sid"":""""}},{{""Action"":[""s3:GetObject""],""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Resource"":[""arn:aws:s3:::{_bucketName}/Authorized/*""],""Sid"":""""}}]}}";
            var args = new SetPolicyArgs()
                .WithBucket(_bucketName)
                .WithPolicy(policyJson);
            await client.SetPolicyAsync(args);
        }


        public async Task GetObjectAsync(string path, string fileName, string? bucketName = null)
        {
            using var client = GetMinioClient(_connections);

            StatObjectArgs statObjectArgs = new StatObjectArgs()
                                  .WithBucket(_bucketName ?? bucketName)
                                  .WithObject(fileName);
            var state = await client.StatObjectAsync(statObjectArgs);

            using (FileStream ms = new FileStream(Path.Combine(path, fileName), FileMode.OpenOrCreate, FileAccess.Write))
            {
                var body = new GetObjectArgs();
                body.WithBucket(_bucketName ?? bucketName)
                    .WithObject(fileName)
                    .WithLength(state.Size)
                    .WithOffsetAndLength(0, state.Size)
                    .WithCallbackStream(sp =>
                    {
                        sp.CopyTo(ms);
                    });

                await client.GetObjectAsync(body);
            }
        }


        public async Task<MemoryStream> GetStreamAsync(string fileName, string? bucketName = null)
        {
            using var client = GetMinioClient(_connections);

            StatObjectArgs statObjectArgs = new StatObjectArgs()
                                  .WithBucket(_bucketName ?? bucketName)
                                  .WithObject(fileName);
            var state = await client.StatObjectAsync(statObjectArgs);

            using MemoryStream ms = new MemoryStream();

            var body = new GetObjectArgs();
            body.WithBucket(_bucketName ?? bucketName)
                .WithObject(fileName)
                .WithLength(state.Size)
                .WithOffsetAndLength(0, state.Size)
                .WithCallbackStream(sp =>
                {
                    sp.CopyTo(ms);
                });

            await client.GetObjectAsync(body);

            return ms;
        }


        public async Task<MemoryStream?> GetStreamAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            using var client = GetMinioClient(_connections);
            string fileName = GetBuckObjectName(url);

            StatObjectArgs statObjectArgs = new StatObjectArgs()
                                .WithBucket(_bucketName)
                                .WithObject(fileName);
            var state = await client.StatObjectAsync(statObjectArgs);

            using MemoryStream ms = new MemoryStream();
            ms.SetLength(state.Size);
            var body = new GetObjectArgs().WithBucket(_bucketName)
                .WithObject(fileName)
                .WithOffsetAndLength(0, state.Size)
                .WithCallbackStream(sp => { sp.CopyTo(ms); ms.Position = 0; });

            await client.GetObjectAsync(body);

            return ms;
        }


        public async Task<string> UploadAsync(string name, byte[] array, string? bucketName = null)
        {
            using var client = GetMinioClient(_connections);
            if (!string.IsNullOrWhiteSpace(bucketName))
                await client.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
            using (Stream ms = new MemoryStream(array))
            {
                var body = new PutObjectArgs()
                 .WithObject(name)
                 .WithObjectSize(ms.Length)
                 .WithStreamData(ms)
                 .WithBucket(bucketName ?? _bucketName)
                 .WithContentType(GetContextType(name));

                await client.PutObjectAsync(body);
            }

            return string.Concat("http://" + _connections.Connection.FirstOrDefault()!.Endpoint + "/", _bucketName + "/", name);
        }


        #region private

        /// <summary>
        /// 获取minio实例
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        private MinioClient GetMinioClient(MinioOptions connections)
        {
            var client = new MinioClient();
            foreach (var item in _connections.Connection!)
                client.WithEndpoint(item.Endpoint).WithCredentials(item.AccessKey, item.SecretKey);

            return client.Build();
        }

        /// <summary>
        /// 根据拓展名获取，context type
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetContextType(string key)
        {
            key = Path.GetExtension(key).ToLower().Trim();
            if (ContextType.Item.ContainsKey(key))
                return ContextType.Item[key].Trim();
            else
                return "application/octet-stream";
        }

        /// <summary>
        /// 解析图片地址
        /// </summary>
        /// <param name="url">http://192.168.1.1:9090/bucketName/test/123.jpg 或 bucketName/test/123.jpg</param>
        /// <returns>桶名，文件名</returns>
        private string GetBuckObjectName(string url)
        {
            return url.Replace(_bucketName + "/", "");
        }

        #endregion
    }
}