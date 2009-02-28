namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Alert, SubCommand.Confirmation)]
    public class AlertConfirmation : ConformationMessageBase<PLCIParameter> {
        public AlertConfirmation() : base(new PLCIParameter()) { }

        internal override void Notify(CapiApplication application, MessageAsyncResult result) {
            Controller controller = application.GetControllerByID(Identifier.ControllerID);
            Connection connection = controller.GetConnectionByPLCI(Identifier.PLCI);
            connection.AlertConfirmation(this, result);
        }
    }
}
