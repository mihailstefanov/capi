namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Listen, SubCommand.Request)]
    public class ListenRequest : RequestMessageBase<ControllerParameter> {
        public ListenRequest(uint controller)
            : base(new ControllerParameter()) {
            Identifier.Value = controller;

            Parameter<int> infoMask = new Parameter<int>();
            infoMask.Value = 0x7F; ;
            ParameterCollection.Add(infoMask);

            Parameter<int> cipMask = new Parameter<int>();
            cipMask.Value = 0x10012;
            ParameterCollection.Add(cipMask);

            //CIP Mask 2
            ParameterCollection.Add(new Parameter<int>());

            ParameterCollection.Add(new PrefixedStringParameter(new byte[] { 0, 0x80 }));
            ParameterCollection.Add(new PrefixedStringParameter(new byte[] { 0x80 }));
        }

        public CIPMask CIPMask {
            get { return (CIPMask)((Parameter<int>)(ParameterCollection[2])).Value; }
            set { ((Parameter<int>)ParameterCollection[2]).Value = (int)value; }
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
