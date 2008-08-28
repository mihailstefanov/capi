namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    internal class WordSerializer : TypeSerializer<short> {
        public override short Read(BinaryReader reader) {
            return reader.ReadInt16();
        }

        public override void Write(BinaryWriter writer, short value) {
            writer.Write(value);
        }

        public override byte GetLength(short value) {
            return 2;
        }
    }
}
