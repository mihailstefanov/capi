namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Diagnostics;

    public class BProtocol : IParameter {
        private B1Protocol _b1;
        private B2Protocol _b2;
        private B3Protocol _b3;


        public B1Protocol B1 {
            get { return _b1; }
            set { _b1 = value; }
        }

        public B2Protocol B2 {
            get { return _b2; }
            set { _b2 = value; }
        }

        public B3Protocol B3 {
            get { return _b3; }
            set { _b3 = value; }
        }

        void ICapiSerializable.Read(BinaryReader reader) {
            byte lenght = reader.ReadByte();
            Debug.Assert(lenght == Length);
            _b1 = (B1Protocol)reader.ReadInt16();
            _b2 = (B2Protocol)reader.ReadInt16();
            _b3 = (B3Protocol)reader.ReadInt16();
        }

        public byte Length {
            get { return 9; }
        }

        void ICapiSerializable.Write(BinaryWriter writer) {
            writer.Write(Length);
            writer.Write((Int16)_b1);
            writer.Write((Int16)_b2);
            writer.Write((Int16)_b3);
            writer.Write(new byte[4]);
        }
    }
}
