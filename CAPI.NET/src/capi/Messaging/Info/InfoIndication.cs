namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Info, SubCommand.Indication)]
    public class InfoIndication : IndicationMessageBase<PLCIParameter> {

        public InfoIndication()
            : base(new PLCIParameter()) {
            ParameterCollection.Add(new Parameter<short>());
        }

        internal override void Notify(CapiApplication application) {
            InfoResponse response = new InfoResponse(this);
            application.SendMessage(response);
        }
    }
}
