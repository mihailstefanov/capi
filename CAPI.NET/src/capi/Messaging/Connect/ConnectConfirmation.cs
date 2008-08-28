using System;
using System.Collections.Generic;
using System.Text;

namespace Mommosoft.Capi {
    [MessageIdentity(Command.Connect, SubCommand.Confirmation)]
    public class ConnectConfirmation : ConformationMessageBase<PLCIParameter> {
        public ConnectConfirmation() : base(new PLCIParameter()) { }

        internal override void Notify(CapiApplication application, MessageAsyncResult result) {
            Controller c = (Controller)result.Caller;
            c.ConnectConfirmation(this, result);
        }
    }
}
