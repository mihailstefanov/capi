namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.ConnectB3, SubCommand.Request)]
    public class ConnectB3Request : RequestMessageBase<PLCIParameter> {
        public ConnectB3Request()
            : base(new PLCIParameter()) {
            // NCPI
            ParameterCollection.Add(new Parameter<byte>());
        }
    }
}
