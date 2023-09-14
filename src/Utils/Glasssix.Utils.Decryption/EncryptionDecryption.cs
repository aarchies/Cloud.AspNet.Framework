using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Glasssix.Utils.Decryption
{
    public static class EncryptionDecryption
    {
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptStr">要解密的串</param>
        /// <param name="aesKey">密钥</param>
        /// <param name="aesIV">IV</param>
        /// <returns></returns>
        public static string AesDecrypt(string decryptStr, string aesKey, string aesIV)
        {
            byte[] byteKEY = Encoding.UTF8.GetBytes(aesKey);
            byte[] byteIV = Encoding.UTF8.GetBytes(aesIV);

            byte[] byteDecrypt = Convert.FromBase64String(decryptStr);

            var _aes = new RijndaelManaged();
            _aes.Padding = PaddingMode.PKCS7;
            _aes.Mode = CipherMode.CBC;

            _aes.Key = byteKEY;
            _aes.IV = byteIV;

            var _crypto = _aes.CreateDecryptor(byteKEY, byteIV);
            byte[] decrypted = _crypto.TransformFinalBlock(
                byteDecrypt, 0, byteDecrypt.Length);

            _crypto.Dispose();

            return Encoding.UTF8.GetString(decrypted);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptStr">要解密的串</param>
        /// <param name="aesKey">密钥</param>
        /// <returns></returns>
        public static string AesDecryptECB(string decryptStr, string aesKey, CipherMode mode, PaddingMode padding)
        {
            byte[] byteKEY = Encoding.UTF8.GetBytes(aesKey);
            byte[] byteDecrypt = Convert.FromBase64String(decryptStr);

            var _aes = new RijndaelManaged
            {
                Mode = mode,
                Padding = padding,
                Key = byteKEY
            };

            var _crypto = _aes.CreateDecryptor();
            byte[] decrypted = _crypto.TransformFinalBlock(byteDecrypt, 0, byteDecrypt.Length);

            _crypto.Dispose();

            return Encoding.UTF8.GetString(decrypted);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="content">输入的数据</param>
        /// <param name="key">加密密钥</param>
        /// <param name="viStr">向量128位</param>
        /// <returns></returns>
        public static string AesEncrypt(string content, string key, string viStr)
        {
            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(content);
            //设置密钥及密钥向量
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = Encoding.UTF8.GetBytes(viStr);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组
                    cs.Close();
                    ms.Close();
                    return Convert.ToBase64String(cipherBytes);
                }
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content">要加密的串</param>
        /// <param name="aesKey">密钥</param>
        /// <returns></returns>
        public static string AesEncryptECB(string content, string aesKey, CipherMode mode, PaddingMode padding)
        {
            byte[] byteKEY = Encoding.UTF8.GetBytes(aesKey);

            byte[] byteContnet = Encoding.UTF8.GetBytes(content);

            var _aes = new RijndaelManaged
            {
                Mode = mode,
                Padding = padding,
                Key = byteKEY
            };

            var _crypto = _aes.CreateEncryptor();
            byte[] decrypted = _crypto.TransformFinalBlock(byteContnet, 0, byteContnet.Length);
            _crypto.Dispose();
            return Convert.ToBase64String(decrypted);
        }

        /// <summary>
        /// AES向量转化
        /// </summary>
        /// <param name="viStr"></param>
        /// <returns></returns>
        public static string ConversionAesVi(string viStr)
        {
            var viChar = viStr.ToArray();
            string result = string.Join(string.Empty, Enumerable.Range(0, 16).Select(p => viChar.Skip(p * 2).Take(2).LastOrDefault()));
            return result;
        }

        /// <summary>
        /// 获取时间戳10位（秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetTimestamp10(this DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>
        /// 方法二
        /// HashAlgorithm加密
        /// 这种加密是  字母加-加字符
        /// Example: appid=CAkJ1LS52MT1&secret=CAkJWlBdQ4Po7FhE3sCjDl3W263f9cuBlrbb9MAZ&timestamp=15694979869591151
        /// 加密变成后 FB069B1E7E76D7DBE38210C4EAAC6886
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <returns></returns>
        public static string HashEncrypt(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            string sign = null;
            try
            {
                byte[] clearBytes = Encoding.ASCII.GetBytes(str);
                byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
                sign = string.Join(string.Empty, hashedBytes?.Select(p => p.ToString("X2")));
            }
            catch (Exception ex)
            {
                Console.WriteLine("HashEncrypt Error", ex);
            }
            return sign;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <param name="upper">是否大写，默认大写</param>
        /// <returns></returns>
        public static string Md5Encrypt(string str, bool upper = true)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;
            byte[] s = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str));
            return string.Join(string.Empty, s?.Select(p => p.ToString(upper ? "X2" : "x2")));
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <returns></returns>
        public static string MD5Encrypt(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            string sign = null;
            try
            {
                byte[] s = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(str));
                sign = string.Join(string.Empty, s?.Select(p => p.ToString("X2")));
            }
            catch (Exception ex)
            {
                Console.WriteLine("MD5Encrypt Error", ex);
            }
            return sign;
        }

        public static string MD5HashCore(string str) => MD5Encrypt(HashEncrypt(str));

        /// <summary>
        /// 根据时间戳获取
        /// </summary>
        /// <param name="datestr"></param>
        /// <returns></returns>
        public static DateTime ToLocalTimeDateBySeconds(long unix)
        {
            var dto = DateTimeOffset.FromUnixTimeMilliseconds(unix);
            return dto.ToLocalTime().DateTime;
        }

        ///// <summary>
        ///// AES解密
        ///// </summary>
        ///// <param name="content">输入的数据 base64</param>
        ///// <param name="key">加密密钥</param>
        ///// <param name="viStr">向量128位</param>
        ///// <returns></returns>
        //public static string AesDecrypt(string content, string key, string viStr)
        //{
        //    SymmetricAlgorithm des = Rijndael.Create();
        //    des.Key = Encoding.UTF8.GetBytes(key);
        //    des.IV = Encoding.UTF8.GetBytes(viStr);
        //    var inputData = Convert.FromBase64String(content);
        //    byte[] decryptBytes = new byte[inputData.Length];
        //    using MemoryStream ms = new MemoryStream(inputData);
        //    using CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
        //    cs.Read(decryptBytes, 0, decryptBytes.Length);
        //    cs.Close();
        //    ms.Close();
        //    var fssf = Encoding.UTF8.GetString(decryptBytes);
        //    var fssaf = Encoding.UTF8.GetString(decryptBytes).TrimEnd('\0')+"\"}";
        //    return fssaf;
        //}
    }
}