using System;
using System.Runtime.InteropServices;

namespace Glasssix.Contrib.Message.Emqx.C
{
    /// <summary>
    /// An internal marshaler for return values that are "string".
    /// </summary>
    internal class TartarusStringMarshaler : ICustomMarshaler
    {
        static ICustomMarshaler GetInstance(string cookie) => new TartarusStringMarshaler();

        public void CleanUpManagedData(object managedObj)
        {
        }

        public void CleanUpNativeData(IntPtr nativeData) => lib_tartarus.TartarusFree(nativeData);

        public int GetNativeDataSize() => -1;

        public IntPtr MarshalManagedToNative(object managedObj) => throw new NotImplementedException();

        public object MarshalNativeToManaged(IntPtr nativeData)
        {
            return nativeData != null ? Marshal.PtrToStringAnsi(nativeData) : null;
        }
    }
}
