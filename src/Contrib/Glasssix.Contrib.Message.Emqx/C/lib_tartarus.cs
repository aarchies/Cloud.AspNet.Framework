using System;
using System.Runtime.InteropServices;

namespace Glasssix.Contrib.Message.Emqx.C
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TartarusBuffer
    {
        public IntPtr Data;
        public UIntPtr Size;
    }
    /// <summary>
    /// 
    /// </summary>
    public static class lib_tartarus
    {
        /// <summary>
        /// Allocates a memory block.
        /// </summary>
        /// <param name="size">The size in bytes</param>
        /// <returns></returns>
        [DllImport("libTartarus", EntryPoint = "tartarus_alloc")]
        public static extern IntPtr TartarusAlloc(ulong size);

        /// <summary>
        /// Frees a memory block which is allocated by TartarusAlloc method.
        /// </summary>
        /// <param name="memory">The memory pointer</param>
        [DllImport("libTartarus", EntryPoint = "tartarus_free")]
        public static extern void TartarusFree(IntPtr memory);

        /// <summary>
        /// Frees a internal buffer.
        /// </summary>
        /// <param name="buffer">The buffer</param>
        [DllImport("libTartarus", EntryPoint = "tartarus_free_buffer")]
        public static extern void TartarusFreeBuffer(ref TartarusBuffer buffer);

        /// <summary>
        /// Encrypts a piece of plaintext.
        /// </summary>
        /// <param name="plaintext">The plaintext</param>
        /// <param name="key">The key</param>
        /// <returns>The ciphertext</returns>
        [DllImport("libTartarus", EntryPoint = "tartarus_encrypt", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(TartarusStringMarshaler))]
        public static extern string TartarusEncrypt(
            [MarshalAs(UnmanagedType.LPStr)] string plaintext,
            [MarshalAs(UnmanagedType.LPStr)] string key
            );

        /// <summary>
        /// Decrypts a piece of ciphertext.
        /// </summary>
        /// <param name="ciphertext">The ciphertext</param>
        /// <param name="key">The key</param>
        /// <returns>The plaintext</returns>
        [DllImport("libTartarus", EntryPoint = "tartarus_decrypt", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(TartarusStringMarshaler))]
        public static extern string TartarusDecrypt(
            [MarshalAs(UnmanagedType.LPStr)] string ciphertext,
            [MarshalAs(UnmanagedType.LPStr)] string key
            );

        /// <summary>
        /// Encrypt a piece of plaintext with the default key.
        /// </summary>
        /// <param name="plaintext">The plaintext</param>
        /// <returns>The ciphertext</returns>
        [DllImport("libTartarus", EntryPoint = "tartarus_encrypt_default", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(TartarusStringMarshaler))]
        public static extern string TartarusEncryptDefault(
            [MarshalAs(UnmanagedType.LPStr)] string plaintext
            );

        /// <summary>
        /// Decrypts a piece of ciphertext with the default key.
        /// </summary>
        /// <param name="ciphertext">The ciphertext</param>
        /// <returns>The plaintext</returns>
        [DllImport("libTartarus", EntryPoint = "tartarus_decrypt_default", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(TartarusStringMarshaler))]
        public static extern string TartarusDecryptDefault(
            [MarshalAs(UnmanagedType.LPStr)] string ciphertext
            );

        /// <summary>
        /// Encrypts a buffer with bytes.
        /// </summary>
        /// <param name="buffer">The buffer</param>
        /// <param name="key">The key</param>
        /// <returns>The ciphertext</returns>
        [DllImport("libTartarus", EntryPoint = "tartarus_encrypt_bytes", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(TartarusStringMarshaler))]
        public static extern string TartarusEncryptBytes(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(TartarusBufferMarshaler))] byte[] buffer,
            [MarshalAs(UnmanagedType.LPStr)] string key
            );

        /// <summary>
        /// Decrypts a piece of ciphertext as bytes.
        /// </summary>
        /// <param name="ciphertext">The ciphertext</param>
        /// <param name="key">The key</param>
        /// <returns>The bytes</returns>
        [DllImport("libTartarus", EntryPoint = "tartarus_decrypt_as_bytes", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(TartarusBufferMarshaler))]
        public static extern byte[] TartarusDecryptAsBytes(
            [MarshalAs(UnmanagedType.LPStr)] string ciphertext,
            [MarshalAs(UnmanagedType.LPStr)] string key);

        /// <summary>
        /// Encrypts a buffer with bytes and the default key.
        /// </summary>
        /// <param name="buffer">The buffer</param>
        /// <returns>The ciphertext</returns>
        [DllImport("libTartarus", EntryPoint = "tartarus_encrypt_bytes_default", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(TartarusStringMarshaler))]
        public static extern string TartarusEncryptBytesDefault(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(TartarusBufferMarshaler))] byte[] buffer
            );

        /// <summary>
        /// Decrypts a piece of ciphertext as bytes with the default key.
        /// </summary>
        /// <param name="ciphertext">The ciphertext</param>
        /// <returns>The bytes</returns>
        [DllImport("libTartarus", EntryPoint = "tartarus_decrypt_as_bytes_default", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(TartarusBufferMarshaler))]
        public static extern byte[] TartarusDecryptAsBytesDefault(
            [MarshalAs(UnmanagedType.LPStr)] string ciphertext
            );
    }
}
