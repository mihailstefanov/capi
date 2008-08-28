namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.DataB3, SubCommand.Request)]
    public class DataB3Request : RequestMessageBase<NCCIParameter> {
        public DataB3Request()
            : base(new NCCIParameter()) {
            // Data.
            ParameterCollection.Add(new Parameter<int>());
            // Data Length.
            ParameterCollection.Add(new Parameter<short>());
            // Data Handle.
            ParameterCollection.Add(new Parameter<short>());
            // Flags.
            ParameterCollection.Add(new Parameter<short>());
            ParameterCollection.Add(new Parameter<Int64>());
        }

        public short DataLength {
            get { return ((Parameter<short>)ParameterCollection[2]).Value; }
            set { ((Parameter<short>)ParameterCollection[2]).Value = value; }
        }

        public short DataHandle {
            get { return ((Parameter<short>)ParameterCollection[3]).Value; }
            set { ((Parameter<short>)ParameterCollection[3]).Value = value; }
        }

        public IntPtr Data {
            get {
                if (IntPtr.Size == 4) {
                    return (IntPtr)(((Parameter<int>)ParameterCollection[1]).Value);
                } else if (IntPtr.Size == 8) {
                    return (IntPtr)(((Parameter<Int64>)ParameterCollection[5]).Value);
                }
                throw new NotSupportedException();
            }
            set {
                if (IntPtr.Size == 4) {
                    (((Parameter<int>)ParameterCollection[1]).Value) = (Int32)value;
                } else if (IntPtr.Size == 8) {
                    (((Parameter<Int64>)ParameterCollection[5]).Value) = (Int64)value;
                } else {
                    throw new NotSupportedException();
                }
            }
        }
    }
}