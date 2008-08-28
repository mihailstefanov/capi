using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mommosoft.Capi {
    public class PrefixedStringParameter : Parameter<string> {
        private byte[] _prefix;

        public PrefixedStringParameter(byte[] prefix)
            : base() {
            _prefix = (prefix == null) ? new byte[0] : prefix;

        }

        protected override void Write(BinaryWriter writer) {
            if (string.IsNullOrEmpty(Value)) {
                writer.Write((byte)0);
                return;
            }

            byte length = (byte)_prefix.Length;
            byte[] textBytes = Encoding.ASCII.GetBytes(Value);
            length += (byte)textBytes.Length;

            // write lenghth.
            writer.Write(length);

            // write prefix.
            if (_prefix.Length != 0) {
                writer.Write(_prefix);
            }

            // write value.
            writer.Write(textBytes);
        }

        protected override void Read(BinaryReader reader) {
            byte length = reader.ReadByte();
            if (length == 0) return;
            for (int i = 0; i < _prefix.Length; i++) {
                _prefix[i] = reader.ReadByte();
            }
            byte[] bytes = reader.ReadBytes(length - (byte)_prefix.Length);
            Value = Encoding.ASCII.GetString(bytes);
        }
    }
}
