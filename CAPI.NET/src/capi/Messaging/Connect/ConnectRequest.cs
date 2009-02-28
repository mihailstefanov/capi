using System;
using System.Collections.Generic;
using System.Text;

namespace Mommosoft.Capi {
    [MessageIdentity(Command.Connect, SubCommand.Request)]
    public class ConnectRequest : RequestMessageBase<ControllerParameter> {
        public ConnectRequest(uint controller)
            : base(new ControllerParameter()) {
            Identifier.Value = controller;
            // CIP Value
            ParameterCollection.Add(new Parameter<ushort>());
            // Called party number
            ParameterCollection.Add(new PrefixedStringParameter(new byte[] { 128 }));
            // Calling party number
            ParameterCollection.Add(new PrefixedStringParameter(new byte[] { 1, 128 }));
            // Called party subaddress
            ParameterCollection.Add(new Parameter<string>());
            // Calling party subaddress
            ParameterCollection.Add(new Parameter<string>());
            // B protocol
            ParameterCollection.Add(new BProtocol());
            // BC
            ParameterCollection.Add(new Parameter<byte>());
            // LLC
            ParameterCollection.Add(new Parameter<byte>());
            // HLC
            ParameterCollection.Add(new Parameter<byte>());
            // Additional Info
            ParameterCollection.Add(new Parameter<byte>());

        }

        public ushort CIPValue {
            get { return ((Parameter<ushort>)ParameterCollection[1]).Value; }
            set { ((Parameter<ushort>)ParameterCollection[1]).Value = value; }
        }

        public string CalledPartyNumber {
            get { return ((Parameter<string>)ParameterCollection[2]).Value; }
            set { ((Parameter<string>)ParameterCollection[2]).Value = value; }
        }

        public string CallingPartyNumber {
            get { return ((Parameter<string>)ParameterCollection[3]).Value; }
            set { ((Parameter<string>)ParameterCollection[3]).Value = value; }
        }

        public BProtocol BPtotocol {
            get { return ((BProtocol)ParameterCollection[6]); }
        }
    }
}
