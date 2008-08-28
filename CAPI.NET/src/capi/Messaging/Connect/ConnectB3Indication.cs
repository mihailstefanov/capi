namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.ConnectB3, SubCommand.Indication)]
    public class ConnectB3Indication : IndicationMessageBase<NCCIParameter> {
        public ConnectB3Indication()
            : base(new NCCIParameter()) {
            // NCPI
            ParameterCollection.Add(new Parameter<byte>());
        }

        internal override void Notify(CapiApplication application) {
            Controller ccontroller = application.GetControllerByID(Identifier.ControllerID);
            Connection connection = ccontroller.GetConnectionByID(Identifier.PLCI);
            connection.ConnectB3Indication(this);
        }
    }
}
