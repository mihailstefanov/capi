namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.ConnectB3Active, SubCommand.Response)]
    public class ConnectB3ActiveResponse : ResponseMessageBase<NCCIParameter> {
        public ConnectB3ActiveResponse(ConnectB3ActiveIndication indication)
            : base(new NCCIParameter(), indication) {

        }
    }
}
