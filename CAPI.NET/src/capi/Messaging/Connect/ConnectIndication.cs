namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Connect, SubCommand.Indication)]
    public class ConnectIndication : IndicationMessageBase<PLCIParameter> {
        public ConnectIndication()
            : base(new PLCIParameter()) {
            // CIP Value
            ParameterCollection.Add(new Parameter<short>());
            // Called party number
            ParameterCollection.Add(new PrefixedStringParameter(new byte[] { 128 }));
            // Calling party number
            ParameterCollection.Add(new PrefixedStringParameter(new byte[] { 0, 128 }));
            // Called party subaddress
            ParameterCollection.Add(new Parameter<string>());
            // Calling party subaddress
            ParameterCollection.Add(new Parameter<string>());

            // BC
            ParameterCollection.Add(new Parameter<byte>());
            // LLC
            ParameterCollection.Add(new Parameter<byte>());
            // HLC
            ParameterCollection.Add(new Parameter<byte>());
            // Additional Info
            ParameterCollection.Add(new Parameter<byte>());

        }

        public short CIPValue {
            get { return ((Parameter<short>)ParameterCollection[1]).Value; }
        }

        public string CalledPartyNumber {
            get { return ((Parameter<string>)ParameterCollection[2]).Value; }
        }

        public string CallingPartyNumber {
            get { return ((Parameter<string>)ParameterCollection[3]).Value; }
        }

        internal override void Notify(CapiApplication application) {
            Controller c = application.Controllers.GetControllerByID(Identifier.ControllerID);
            c.ConnectIndication(this);
        }
    }
}
