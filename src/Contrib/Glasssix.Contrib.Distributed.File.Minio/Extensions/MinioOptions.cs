using System.Collections.Generic;

namespace Glasssix.Contrib.File.Storage.Extensions
{
    public class MinioOptions
    {
        public MinioOptions()
        {

        }

        public MinioOptions(DefaultMinioConnection connection, string bucketName)
        {
            Connection!.Add(connection);
            BucketName = bucketName;
        }

        public MinioOptions(List<DefaultMinioConnection> connection, string bucketName)
        {
            Connection!.AddRange(connection);
            BucketName = bucketName;
        }

        public List<DefaultMinioConnection>? Connection { get; set; } = new List<DefaultMinioConnection>();
        public string? BucketName { get; set; }
    }


    public class DefaultMinioConnection
    {
        public string? Endpoint { get; set; }

        public string? AccessKey { get; set; }

        public string? SecretKey { get; set; }
    }
}