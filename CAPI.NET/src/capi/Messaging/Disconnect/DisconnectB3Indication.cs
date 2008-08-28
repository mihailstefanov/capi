namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.DisconnectB3, SubCommand.Indication)]
    public class DisconnectB3Indication : IndicationMessageBase<NCCIParameter> {
        public DisconnectB3Indication() : base(new NCCIParameter()) { }

        internal override void Notify(CapiApplication application) {
            Controller controller = application.Controllers.GetControllerByID(Identifier.ControllerID);
            Connection connection = controller.Connections.GetConnectionByID(Identifier.PLCI);
            connection.DisconnectB3Indication(this);
        }
    }
}
