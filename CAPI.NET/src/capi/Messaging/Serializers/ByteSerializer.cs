namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    internal class ByteSerializer : TypeSerializer<byte> {
        public override byte Read(BinaryReader reader) {
            return reader.ReadByte();
        }

        public override void Write(BinaryWriter writer, byte value) {
            writer.Write(value);
        }

        public override byte GetLength(byte value) {
            return 1;
        }
    }
}
