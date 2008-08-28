namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Alert, SubCommand.Request)]
    public class AlertRequest : RequestMessageBase<PLCIParameter> {
        public AlertRequest()
            : base(new PLCIParameter()) {
            // Additional Info
            ParameterCollection.Add(new Parameter<byte>());
        }
    }
}
