namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.ConnectActive, SubCommand.Response)]
    public class ConnectActiveResponse : ResponseMessageBase<PLCIParameter> {
        public ConnectActiveResponse(ConnectActiveIndication indication)
            : base(new PLCIParameter(), indication) {
        }
    }
}
