namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.Specialized;

    public class NCCIParameter : PLCIParameter {
        protected static readonly BitVector32.Section NCCISection = BitVector32.CreateSection(short.MaxValue, PLCISection);

        public NCCIParameter() : this(0) { }

        public NCCIParameter(int value) {
            Value = value;
        }

        public short NCCI {
            get {
                BitVector32 vector = new BitVector32(Value);
                return Convert.ToInt16(vector[NCCISection]);
            }
            set {
                BitVector32 vector = new BitVector32(Value);
                vector[NCCISection] = value;
                Value = vector.Data;
            }
        }

    }
}
