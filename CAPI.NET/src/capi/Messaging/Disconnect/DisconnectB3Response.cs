namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.DisconnectB3, SubCommand.Response)]
    public class DisconnectB3Response : ResponseMessageBase<NCCIParameter> {
        public DisconnectB3Response(DisconnectB3Indication indication)
            : base(new NCCIParameter(), indication) {

        }
    }
}
