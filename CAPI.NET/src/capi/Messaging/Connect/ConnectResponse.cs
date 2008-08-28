namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Connect, SubCommand.Response)]
    public class ConnectResponse : ResponseMessageBase<PLCIParameter> {
        public ConnectResponse(ConnectIndication indication)
            : base(new PLCIParameter(), indication) {
            // Reject
            ParameterCollection.Add(new Parameter<Int16>());

            // B protocol
            ParameterCollection.Add(new BProtocol());

            // Connected Number
            ParameterCollection.Add(new PrefixedStringParameter(new byte[] { 0, 128 }));

            // Connected Subaddress
            ParameterCollection.Add(new PrefixedStringParameter(new byte[] { 128 }));

            // LLC
            ParameterCollection.Add(new Parameter<byte>());

            // Additional Info
            ParameterCollection.Add(new Parameter<byte>());
        }

        public Reject Reject {
            get { return (Reject)((Parameter<Int16>)ParameterCollection[1]).Value; }
            set { ((Parameter<Int16>)ParameterCollection[1]).Value = (Int16)value; }
        }

        public BProtocol BPtotocol {
            get { return ((BProtocol)ParameterCollection[2]); }
        }

        public string ConnectedNumber {
            get { return ((Parameter<string>)ParameterCollection[3]).Value; }
            set { ((Parameter<string>)ParameterCollection[3]).Value = value; }
        }

        public string ConnectedSubaddress {
            get { return ((Parameter<string>)ParameterCollection[4]).Value; }
            set { ((Parameter<string>)ParameterCollection[4]).Value = value; }
        }
    }
}
