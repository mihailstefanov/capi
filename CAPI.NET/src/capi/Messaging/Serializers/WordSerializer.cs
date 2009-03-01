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

    internal class UWordSerializer : TypeSerializer<ushort> {
        public override ushort Read(BinaryReader reader) {
            return reader.ReadUInt16();
        }

        public override void Write(BinaryWriter writer, ushort value) {
            writer.Write(value);
        }

        public override byte GetLength(ushort value) {
            return 2;
        }
    }
}
