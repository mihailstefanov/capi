namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.Specialized;

    public class PLCIParameter : Parameter<uint> {
        public PLCIParameter(uint value) {
            Value = value;
        }

        public PLCIParameter() {
        }

        public uint ControllerID {
            get { return (uint)((Value & (uint)0x000000FF)); }
        }

        //public bool Internal {
        //    get {
        //        BitVector32 vector = new BitVector32(Value);
        //        return Convert.ToBoolean(vector[IntExtSection]);
        //    }
        //}

        public uint PLCI {
            get { return Value; }
            set { Value = value; }
        }
    }
}
