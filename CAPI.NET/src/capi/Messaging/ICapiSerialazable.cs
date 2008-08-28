namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    public interface ICapiSerializable {
        byte Length { get; }
        void Read(BinaryReader reader);
        void Write(BinaryWriter writer);
    }
}
