using Glasssix.Utils.Decryption.Sm4;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Glasssix.Utils.Decryption
{
    public class EncryptionDecryptionProvider
    {
        public EncryptionDecryptionProvider()
        {
        }

        #region AES

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptStr">要解密的串</param>
        /// <param name="aesKey">密钥</param>
        /// <param name="aesIV">IV</param>
        /// <returns></returns>
        public string AesDecrypt(string decryptStr, string aesKey, string aesIV)
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
        public string AesDecryptECB(string decryptStr, string aesKey, CipherMode mode, PaddingMode padding)
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
        public string AesEncrypt(string content, string key, string viStr)
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
        public string AesEncryptECB(string content, string aesKey, CipherMode mode, PaddingMode padding)
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

        #endregion AES

        #region MD5

        /// <summary>
        /// AES向量转化
        /// </summary>
        /// <param name="viStr"></param>
        /// <returns></returns>
        public string ConversionAesVi(string viStr)
        {
            var viChar = viStr.ToArray();
            string result = string.Join(string.Empty, Enumerable.Range(0, 16).Select(p => viChar.Skip(p * 2).Take(2).LastOrDefault()));
            return result;
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
        public string HashEncrypt(string str)
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

        public string MD5Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            des.Key = Encoding.ASCII.GetBytes(sKey);
            des.IV = Encoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <param name="upper">是否大写，默认大写</param>
        /// <returns></returns>
        public string Md5Encrypt(string str, bool upper = true)
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
        public string MD5Encrypt(string str)
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

        public string MD5HashCore(string str) => MD5Encrypt(HashEncrypt(str));

        /// <summary>
        /// 根据时间戳获取
        /// </summary>
        /// <param name="datestr"></param>
        /// <returns></returns>
        public DateTime ToLocalTimeDateBySeconds(long unix)
        {
            var dto = DateTimeOffset.FromUnixTimeMilliseconds(unix);
            return dto.ToLocalTime().DateTime;
        }

        #endregion MD5

        #region Sm4

        #region 加密

        /// <summary>
        /// CBC加密
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SM4EncryptCBC(string key, string iv, string data, bool HexString = false)
        {
            Sm4Context ctx = new Sm4Context
            {
                IsPadding = true
            };

            byte[] keyBytes = HexString ? Hex.Decode(key) : Encoding.Default.GetBytes(key);

            byte[] ivBytes = HexString ? Hex.Decode(iv) : Encoding.Default.GetBytes(iv);

            SM4 sm4 = new SM4();
            sm4.SetKeyEnc(ctx, keyBytes);
            byte[] encrypted = sm4.Sm4CryptCbc(ctx, ivBytes, Encoding.Default.GetBytes(data));
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// ECB加密
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SM4EncryptECB(string key, string data, bool HexString = false)
        {
            Sm4Context ctx = new Sm4Context
            {
                IsPadding = true
            };

            byte[] keyBytes = HexString ? Hex.Decode(key) : Encoding.Default.GetBytes(key);

            SM4 sm4 = new SM4();
            sm4.SetKeyEnc(ctx, keyBytes);
            byte[] encrypted = sm4.Sm4CryptEcb(ctx, Encoding.Default.GetBytes(data));

            return Encoding.Default.GetString(Hex.Encode(encrypted));
        }

        #endregion 加密

        #region 解密

        /// <summary>
        /// CBC解密
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public string SM4DecryptCBC(string key, string iv, string data, bool HexString = false)
        {
            Sm4Context ctx = new Sm4Context
            {
                IsPadding = true,
                Mode = 0
            };

            byte[] keyBytes = HexString ? Hex.Decode(key) : Encoding.Default.GetBytes(key);
            byte[] ivBytes = HexString ? Hex.Decode(iv) : Encoding.Default.GetBytes(iv);

            SM4 sm4 = new SM4();
            sm4.Sm4SetKeyDec(ctx, keyBytes);
            byte[] decrypted = sm4.Sm4CryptCbc(ctx, ivBytes, Convert.FromBase64String(data));
            return Encoding.Default.GetString(decrypted);
        }

        /// <summary>
        ///  ECB解密
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string SM4DecryptECB(string key, string data, bool HexString = false)
        {
            Sm4Context ctx = new Sm4Context
            {
                IsPadding = true,
                Mode = 0
            };

            byte[] keyBytes = HexString ? Hex.Decode(key) : Encoding.Default.GetBytes(data);

            SM4 sm4 = new SM4();
            sm4.Sm4SetKeyDec(ctx, keyBytes);
            byte[] decrypted = sm4.Sm4CryptEcb(ctx, Hex.Decode(data));
            return Encoding.Default.GetString(decrypted);
        }

        #endregion 解密

        #endregion Sm4
    }
}