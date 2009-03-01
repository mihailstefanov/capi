using System;
using System.Collections.Generic;
using System.Text;

namespace Mommosoft.Capi {
    [MessageIdentity(Command.Disconnect, SubCommand.Request)]
    public class DisconnectRequest : RequestMessageBase<PLCIParameter> {
        public DisconnectRequest(uint plci) : base(new PLCIParameter(plci)) {

            // Additional Info
            ParameterCollection.Add(new StructParameter());
        }
    }
}
