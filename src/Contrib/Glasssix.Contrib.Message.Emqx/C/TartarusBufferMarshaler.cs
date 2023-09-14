using System;
using System.Runtime.InteropServices;

namespace Glasssix.Contrib.Message.Emqx.C
{
    /// <summary>
    /// An internal marshaler for return values and arguments that are "TartarusBuffer".
    /// </summary>
    internal class TartarusBufferMarshaler : ICustomMarshaler
    {
        static ICustomMarshaler GetInstance(string cookie) => new TartarusBufferMarshaler();

        public void CleanUpManagedData(object managedObj)
        {
        }

        public void CleanUpNativeData(IntPtr nativeData)
        {
            var buffer = Marshal.PtrToStructure<TartarusBuffer>(nativeData);

            lib_tartarus.TartarusFreeBuffer(ref buffer);
            lib_tartarus.TartarusFree(nativeData);
        }

        public int GetNativeDataSize() => -1;

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            var nativeBuffer = new TartarusBuffer();

            if (managedObj is byte[] buffer)
            {
                nativeBuffer.Size = (UIntPtr)buffer.Length;
                nativeBuffer.Data = lib_tartarus.TartarusAlloc(nativeBuffer.Size.ToUInt64());

                Marshal.Copy(buffer, 0, nativeBuffer.Data, buffer.Length);
            }

            var result = lib_tartarus.TartarusAlloc(Convert.ToUInt64(Marshal.SizeOf(typeof(TartarusBuffer))));

            Marshal.StructureToPtr(nativeBuffer, result, false);

            return result;
        }

        public object MarshalNativeToManaged(IntPtr nativeData)
        {
            if (nativeData == null)
            {
                return null;
            }

            var buffer = Marshal.PtrToStructure<TartarusBuffer>(nativeData);

            if (buffer.Data == IntPtr.Zero || buffer.Size == UIntPtr.Zero)
            {
                return null;
            }

            var result = new byte[buffer.Size.ToUInt64()];

            Marshal.Copy(buffer.Data, result, 0, result.Length);

            return result;
        }
    }
}
