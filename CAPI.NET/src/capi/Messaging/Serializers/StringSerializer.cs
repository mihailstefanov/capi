namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    internal class StringSerializer : TypeSerializer<string> {
        public override string Read(BinaryReader reader) {
            return reader.ReadString();
        }

        public override void Write(BinaryWriter writer, string value) {
            if (string.IsNullOrEmpty(value)) {
                writer.Write((byte)0);
                return;
            }
            writer.Write(value);
        }

        public override byte GetLength(string value) {
            if (string.IsNullOrEmpty(value)) return 1;
            return (byte)(Encoding.ASCII.GetByteCount(value) + 1);
        }
    }
}
