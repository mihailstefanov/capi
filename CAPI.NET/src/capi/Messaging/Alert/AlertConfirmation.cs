namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Alert, SubCommand.Confirmation)]
    public class AlertConfirmation : ConformationMessageBase<PLCIParameter> {
        public AlertConfirmation() : base(new PLCIParameter()) { }

        internal override void Notify(CapiApplication application, MessageAsyncResult result) {
            throw new NotImplementedException();
        }
    }
}
