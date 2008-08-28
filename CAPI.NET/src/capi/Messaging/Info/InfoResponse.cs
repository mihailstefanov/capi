namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Info, SubCommand.Response)]
    public class InfoResponse : ResponseMessageBase<PLCIParameter> {
        public InfoResponse(InfoIndication indication) : base(new PLCIParameter(), indication) { }
    }
}
