namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.ObjectModel;

    public class ProtocolCollection : ReadOnlyCollection<Protocol> {
        internal ProtocolCollection(IList<Protocol> protocolList)
            : base(protocolList) {
        }
    }
}
