using System;
using System.Collections.Generic;
using System.Text;

namespace Mommosoft.Capi {
    public class DisconnectRequest : RequestMessageBase<PLCIParameter> {
        public DisconnectRequest(uint plci) : base(new PLCIParameter(plci)) {
        }
    }
}
