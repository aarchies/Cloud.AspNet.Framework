namespace Glasssix.Utils.Decryption.Sm4
{
    /// <summary>
    ///
    /// </summary>
    internal class Sm4Context
    {
        /// <summary>
        /// 是否补足16进制字符串
        /// </summary>
        public bool IsPadding;

        /// <summary>
        /// 密钥
        /// </summary>
        public long[] Key;

        /// <summary>
        /// 1表示加密，0表示解密
        /// </summary>
        public int Mode;

        public Sm4Context()
        {
            Mode = 1;
            IsPadding = true;
            Key = new long[32];
        }
    }
}