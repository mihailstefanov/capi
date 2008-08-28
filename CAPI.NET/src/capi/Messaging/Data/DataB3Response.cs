namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.DataB3, SubCommand.Response)]
    public class DataB3Response : ResponseMessageBase<NCCIParameter> {
        public DataB3Response(DataB3Indication indication)
            : base(new NCCIParameter(), indication) {
            // Data Handle
            Parameter<short> handle = new Parameter<short>();
            handle.Value = indication.Handle;
            ParameterCollection.Add(handle);
        }

        public short Handle {
            get { return ((Parameter<short>)ParameterCollection[1]).Value; }
        }
    }
}

