using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mommosoft.Capi {
    public abstract class TypeSerializer<T> {
        private static TypeSerializer<T> s_defaultSerializer;

        public abstract byte GetLength(T value);
        public abstract T Read(BinaryReader reader);
        public abstract void Write(BinaryWriter writer, T value);

        public static TypeSerializer<T> Default {
            get {
                TypeSerializer<T> serializer = s_defaultSerializer;
                if (serializer == null) {
                    serializer = CreateSerializer();
                    s_defaultSerializer = serializer;
                }
                return serializer;
            }
        }

        private static TypeSerializer<T> CreateSerializer() {
            Type c = typeof(T);
            if (c == typeof(byte)) {
                return (TypeSerializer<T>)(object)new ByteSerializer();
            } else if (c == typeof(short)) {
                return (TypeSerializer<T>)(object)new WordSerializer();
            } else if (c == typeof(ushort)) {
                return (TypeSerializer<T>)(object)new UWordSerializer();
            } else if (c == typeof(int)) {
                return (TypeSerializer<T>)(object)new DWordSerializer();
            } else if (c == typeof(uint)) {
                return (TypeSerializer<T>)(object)new UDWordSerializer();
            } else if (c == typeof(string)) {
                return (TypeSerializer<T>)(object)new StringSerializer();
            } else if (c == typeof(long)) {
                return (TypeSerializer<T>)(object)new QWordSerializer();
            }

            throw new NotSupportedException();
        }
    }
}
