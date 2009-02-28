using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Mommosoft.Capi {

    public class Parameter<T> : IParameter {
        private T _value;
        private TypeSerializer<T> _serializer;

        public Parameter() {
            _serializer = TypeSerializer<T>.Default;
        }

        public Parameter(T value) : this() {
            _value = value;
        }
        
        public Parameter(TypeSerializer<T> serializer, T value) {
            _value = value;
            _serializer = serializer;
        }

        public T Value {
            get { return _value; }
            set { _value = value; }
        }

        public virtual byte Length {
            get {
                if (_serializer != null)
                    return _serializer.GetLength(Value);
                return (byte)Marshal.SizeOf(_value);
            }
        }

        protected virtual void Read(BinaryReader reader) {
            _value = _serializer.Read(reader);
        }

        void ICapiSerializable.Read(BinaryReader reader) {
            Read(reader);
        }

        protected virtual void Write(BinaryWriter writer) {
            _serializer.Write(writer, _value);
        }

        void ICapiSerializable.Write(BinaryWriter writer) {
            Write(writer);
        }
    }
}
