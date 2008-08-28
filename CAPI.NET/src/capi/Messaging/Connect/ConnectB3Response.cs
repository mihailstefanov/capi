namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.ConnectB3, SubCommand.Response)]
    public class ConnectB3Response : ResponseMessageBase<NCCIParameter> {
        public ConnectB3Response(ConnectB3Indication indication)
            : base(new NCCIParameter(), indication) {
            // Reject
            ParameterCollection.Add(new Parameter<short>());
            // NCPI
            ParameterCollection.Add(new Parameter<byte>());
        }

        public Reject Reject {
            get { return ((Reject)((Parameter<short>)ParameterCollection[1]).Value); }
            set { ((Parameter<short>)ParameterCollection[1]).Value = (short)value; }
        }
    }
}
