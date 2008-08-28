namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.ObjectModel;
    using System.IO;

    public class ParameterCollection : Collection<IParameter>, ICapiSerializable {
        public byte Length {
            get {
                byte length = 0;
                foreach (IParameter parameter in this) {
                    length += parameter.Length;
                }
                return length;
            }
        }


        protected virtual void Read(BinaryReader stream) {
            foreach (IParameter parameter in this) {
                parameter.Read(stream);
            }
        }

        void ICapiSerializable.Read(BinaryReader reader) {
            Read(reader);
        }

        protected virtual void Write(BinaryWriter writer) {
            foreach (IParameter parameter in this) {
                parameter.Write(writer);
            }
        }

        void ICapiSerializable.Write(BinaryWriter writer) {
            Write(writer);
        }
    }
}
