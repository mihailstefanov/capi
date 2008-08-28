namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.Specialized;

    public class PLCIParameter : Parameter<int> {
        protected static readonly BitVector32.Section ControllerSection = BitVector32.CreateSection(127);
        protected static readonly BitVector32.Section IntExtSection = BitVector32.CreateSection(1, ControllerSection);
        protected static readonly BitVector32.Section PLCISection = BitVector32.CreateSection(255, IntExtSection);

        public PLCIParameter(int value) {
            Value = value;
        }

        public PLCIParameter() {
        }

        public int ControllerID {
            get {
                BitVector32 vector = new BitVector32(Value);
                return vector[ControllerSection];
            }
            set {
                BitVector32 vector = new BitVector32(Value);
                vector[ControllerSection] = value;
                Value = vector.Data;
            }
        }

        public bool Internal {
            get {
                BitVector32 vector = new BitVector32(Value);
                return Convert.ToBoolean(vector[IntExtSection]);
            }
        }

        public byte PLCI {
            get {
                BitVector32 vector = new BitVector32(Value);
                return Convert.ToByte(vector[PLCISection]);
            }
            set {
                BitVector32 vector = new BitVector32(Value);
                vector[PLCISection] = value;
                Value = vector.Data;
            }
        }
    }
}
