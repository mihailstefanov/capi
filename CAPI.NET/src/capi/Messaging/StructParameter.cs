using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mommosoft.Capi {
    public class StructParameter : IParameter {
        private byte[] _data;

        public StructParameter() { }
        public StructParameter(byte[] data) { _data = data; } // may be copy is better.

        byte ICapiSerializable.Length {
            get { return (byte)_data.Length; }
        }

        void ICapiSerializable.Read(BinaryReader reader) {
            byte length = reader.ReadByte();
            _data = reader.ReadBytes(length);
        }

        void ICapiSerializable.Write(BinaryWriter writer) {
            byte length = (_data != null) ? (byte)_data.Length : (byte)0;
            writer.Write(length);
            if (length > 0) {
                writer.Write(_data);
            }
        }
    }
}
