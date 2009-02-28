using System;
using System.Collections.Generic;
using System.Text;
using Mommosoft.Capi.Properties;
using System.Runtime.InteropServices;
using System.IO;

namespace Mommosoft.Capi {
    internal static class CapiPInvoke {
        public static int Register(int messageBufferLength,
                            int maxLogicalConnection,
                            int maxBDataBlocks,
                            int maxBDataLength) {
            int appID;

            int code = NativeMethods.CAPI_REGISTER(messageBufferLength, maxLogicalConnection, maxBDataBlocks,
                maxBDataLength, out appID);
            ValidateCode(code);
            return appID;
        }

        public static void Release(int appID) {
            int code = NativeMethods.CAPI_RELEASE(appID);
            ValidateCode(code);
        }

        public static string GetManufacturer() {
            StringBuilder builder = new StringBuilder(64);
            NativeMethods.CAPI_GET_MANUFACTURER(builder);
            return builder.ToString();
        }

        public static Version GetVersion() {
            int major;
            int minor;
            int mMajor;
            int mMinor;
            int code = NativeMethods.CAPI_GET_VERSION(out major, out minor, out mMajor, out mMinor);
            ValidateCode(code);
            return new Version(major, minor, mMajor, mMinor);
        }

        public static string GetSerialNumber() {
            StringBuilder builder = new StringBuilder(8);
            NativeMethods.CAPI_GET_SERIAL_NUMBER(builder);
            return builder.ToString();
        }

        public static int WaitForSignal(int appID) {
            int code = NativeMethods.CAPI_WAIT_FOR_SIGNAL(appID);
            ValidateCode(code);
            return code;
        }

        public static NativeMethods.Profile GetProfile(uint controller) {
            NativeMethods.Profile profile = new NativeMethods.Profile();
            int code = NativeMethods.CAPI_GET_PROFILE(ref profile, controller);
            ValidateCode(code);
            return profile;
        }

        public static int PutMessage(int appID, MemoryStream stream) {
            IntPtr ptr = Marshal.AllocHGlobal((int)stream.Length);

            int code;
            try {
                Marshal.Copy(stream.GetBuffer(), 0, ptr, (int)stream.Length);
                code = NativeMethods.CAPI_PUT_MESSAGE(appID, ptr);
            } finally {
                if (ptr != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptr);
                }
            }
            ValidateCode(code);
            return code;
        }

        public static MemoryStream GetMessage(int appID) {
            IntPtr ptr;
            int code = NativeMethods.CAPI_GET_MESSAGE(appID, out ptr);
            ValidateCode(code);
            Int16 length = Marshal.ReadInt16(ptr);
            byte[] bytes = new byte[length];
            Marshal.Copy(ptr, bytes, 0, length);
            return new MemoryStream(bytes);
        }

        public static T MarshalMessage<T>(IntPtr ptr) {
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
        }

        private static void ValidateCode(int code) {
            if ((code & 0x1100) != 0) {
                throw new CapiException((Info)code);
            }
        }

    }
}
