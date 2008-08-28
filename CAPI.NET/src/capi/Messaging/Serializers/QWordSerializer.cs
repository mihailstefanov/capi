namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    class QWordSerializer : TypeSerializer<long> {
        public override byte GetLength(long value) {
            return sizeof(long);
        }

        public override long Read(BinaryReader reader) {
            return reader.ReadInt64();
        }

        public override void Write(BinaryWriter writer, long value) {
            writer.Write(value);
        }
    }
}
