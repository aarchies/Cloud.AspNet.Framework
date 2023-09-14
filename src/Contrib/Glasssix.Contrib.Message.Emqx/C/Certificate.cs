using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Glasssix.Contrib.Message.Emqx.C
{
    public class Certificate
    {
        public enum PemStringType
        {
            Certificate,
            RsaPrivateKey,
            PublicKey
        }
        /// <summary>
        /// Creates X509 certificate
        /// </summary>
        /// <param name="publicCertificate">PEM string of public certificate.</param>
        /// <param name="privateKey">PEM string of private key.</param>
        /// <param name="password">Password for certificate.</param>
        /// <returns>An instance of <see cref="X509Certificate2"/> rapresenting the X509 certificate.</returns>
        public static X509Certificate2 GetCertificateFromPEMstring(string publicCertificate, string privateKey, string password)
        {
            X509Certificate2 certificate = new X509Certificate2(GetBytesFromPemString(publicCertificate, PemStringType.Certificate), password);
            var privateKeyBytes = GetBytesFromPemString(privateKey, PemStringType.RsaPrivateKey);
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
                X509Certificate2 certificateWithKey = certificate.CopyWithPrivateKey(rsa);
                return certificateWithKey;
            }
        }
        public static byte[] GetBytesFromPemString(string pemString, PemStringType type)
        {
            string header, footer;

            switch (type)
            {
                case PemStringType.Certificate:
                    header = "-----BEGIN CERTIFICATE-----";
                    footer = "-----END CERTIFICATE-----";
                    break;
                case PemStringType.RsaPrivateKey:
                    header = "-----BEGIN RSA PRIVATE KEY-----";
                    footer = "-----END RSA PRIVATE KEY-----";
                    break;
                case PemStringType.PublicKey:
                    header = "-----BEGIN PUBLIC KEY-----";
                    footer = "-----END PUBLIC KEY-----";
                    break;
                default:
                    return null;
            }

            int start = pemString.IndexOf(header) + header.Length;
            int end = pemString.IndexOf(footer, start) - start;
            return Convert.FromBase64String(pemString.Substring(start, end));
        }
    }
}
