namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Listen, SubCommand.Request)]
    public class ListenRequest : RequestMessageBase<ControllerParameter> {
        public ListenRequest(uint controller)
            : base(new ControllerParameter()) {
            Identifier.Value = controller;

            // Info mask
            ParameterCollection.Add(new Parameter<uint>());

            // CIP Mask
            ParameterCollection.Add(new Parameter<uint>());

            // CIP Mask 2
            ParameterCollection.Add(new Parameter<int>());

            // Calling party number
            ParameterCollection.Add(new PrefixedStringParameter(new byte[] { 0, 0x80 }));
            ParameterCollection.Add(new PrefixedStringParameter(new byte[] { 0x80 }));
        }

        public CIPMask CIPMask {
            get { return (CIPMask)((Parameter<uint>)(ParameterCollection[2])).Value; }
            set { ((Parameter<uint>)ParameterCollection[2]).Value = (uint)value; }
        }

        public string CallingPartyNumber {
            get { return ((Parameter<string>)ParameterCollection[4]).Value; }
            set { ((Parameter<string>)ParameterCollection[4]).Value = value; }
        }

        public string CallingSubaddress {
            get { return ((Parameter<string>)ParameterCollection[5]).Value; }
            set { ((Parameter<string>)ParameterCollection[5]).Value = value; }
        }
    }
}
