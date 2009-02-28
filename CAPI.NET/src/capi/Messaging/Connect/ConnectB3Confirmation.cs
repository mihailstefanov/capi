namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.ConnectB3, SubCommand.Confirmation)]
    public class ConnectB3Confirmation : ConformationMessageBase<NCCIParameter> {
        public ConnectB3Confirmation() : base(new NCCIParameter()) { }

        internal override void Notify(CapiApplication application, MessageAsyncResult result) {
            Controller controller = application.GetControllerByID(Identifier.ControllerID);
            Connection connection = controller.GetConnectionByPLCI(Identifier.PLCI);
            connection.ConnectB3Confirmation(this, result);
        }
    }
}
