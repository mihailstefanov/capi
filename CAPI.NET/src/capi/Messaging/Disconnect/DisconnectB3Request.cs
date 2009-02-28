namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.DisconnectB3, SubCommand.Request)]
    public class DisconnectB3Request : RequestMessageBase<NCCIParameter> {
        public DisconnectB3Request(uint identifier)
            : base(new NCCIParameter()) {
            Identifier.Value = identifier;
            // NCPI
            ParameterCollection.Add(new Parameter<byte>());
        }
    }
}
