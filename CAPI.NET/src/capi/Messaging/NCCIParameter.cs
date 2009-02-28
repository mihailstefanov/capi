namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.Specialized;

    public class NCCIParameter : Parameter<uint> {

        public NCCIParameter() { }
        public NCCIParameter(uint value) : base(value) { }

        public uint ControllerID {
            get { return (uint)((Value & (uint)0x000000FF)); }
        }

        public uint PLCI {
            get { return (Value & (uint)0x0000FFFF); }
        }

        public uint NCCI {
            get { return Value; }
            set { Value = value; }
        }

    }
}
