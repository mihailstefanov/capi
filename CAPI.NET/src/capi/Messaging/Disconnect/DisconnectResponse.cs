namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Disconnect, SubCommand.Response)]
    public class DisconnectResponse : ResponseMessageBase<PLCIParameter> {
        public DisconnectResponse(DisconnectIndication indication) : base(new PLCIParameter(), indication) { }
    }
}
