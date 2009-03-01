namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.DisconnectB3, SubCommand.Indication)]
    public class DisconnectB3Indication : IndicationMessageBase<NCCIParameter> {
        public DisconnectB3Indication() : base(new NCCIParameter()) { }

        internal override void Notify(CapiApplication application) {
            Controller controller = application.Controllers.GetControllerByID(Identifier.ControllerID);
            if (controller != null) {
                Connection connection = controller.Connections.GetConnectionByPLCI(Identifier.PLCI);
                if (connection != null) {
                    connection.DisconnectB3Indication(this);
                }
            }
        }
    }
}
