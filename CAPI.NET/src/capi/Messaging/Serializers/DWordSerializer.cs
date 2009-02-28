namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    internal class DWordSerializer : TypeSerializer<int> {
        public override int Read(BinaryReader reader) {
            return reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer, int value) {
            writer.Write(value);
        }

        public override byte GetLength(int value) {
            return sizeof(int);
        }
    }

    internal class UDWordSerializer : TypeSerializer<uint> {
        public override uint Read(BinaryReader reader) {
            return reader.ReadUInt32();
        }

        public override void Write(BinaryWriter writer, uint value) {
            writer.Write(value);
        }

        public override byte GetLength(uint value) {
            return sizeof(uint);
        }
    }
}
