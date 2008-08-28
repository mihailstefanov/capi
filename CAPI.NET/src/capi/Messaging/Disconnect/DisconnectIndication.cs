namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Disconnect, SubCommand.Indication)]
    public class DisconnectIndication : IndicationMessageBase<PLCIParameter> {
        public DisconnectIndication()
            : base(new PLCIParameter()) {
            ParameterCollection.Add(new Parameter<short>());
        }

        public Reasons Reason {
            get { return (Reasons)((Parameter<short>)ParameterCollection[1]).Value; }
        }

        internal override void Notify(CapiApplication application) {
            Controller c = application.Controllers.GetControllerByID(Identifier.ControllerID);
            c.DisconnectIndication(this);
        }
    }
}
