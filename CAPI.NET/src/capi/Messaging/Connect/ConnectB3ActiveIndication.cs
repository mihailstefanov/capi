namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.ConnectB3Active, SubCommand.Indication)]
    public class ConnectB3ActiveIndication : ConnectB3Indication {
        public ConnectB3ActiveIndication() { }

        internal override void Notify(CapiApplication application) {
            Controller controller = application.Controllers.GetControllerByID(Identifier.ControllerID);
            Connection connection = controller.Connections.GetConnectionByPLCI(Identifier.PLCI);
            connection.ConnectB3ActiveIndication(this);
        }
    }
}
