namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.DisconnectB3, SubCommand.Confirmation)]
    public class DisconnectB3Confirmation : ConformationMessageBase<NCCIParameter> {
        public DisconnectB3Confirmation() : base(new NCCIParameter()) { }

        internal override void Notify(CapiApplication application, MessageAsyncResult result) {
            Connection connection = (Connection)result.Caller;
            connection.DisconnectB3Confirmation(this, result);
        }
    }
}
