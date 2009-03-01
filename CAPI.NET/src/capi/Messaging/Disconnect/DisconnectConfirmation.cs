using System;
using System.Collections.Generic;
using System.Text;

namespace Mommosoft.Capi {
    [MessageIdentity(Command.Disconnect, SubCommand.Confirmation)]
    public class DisconnectConfirmation : ConformationMessageBase<PLCIParameter> {
        public DisconnectConfirmation() : base(new PLCIParameter()) { }

        internal override void Notify(CapiApplication application, MessageAsyncResult result) {
            Connection connection = (Connection)result.Caller;
            connection.DisconnectConfirmation(this, result);
        }
    }
}
