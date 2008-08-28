namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class IdentifierMessageBase<T> : Message where T : Parameter<int> {
        protected IdentifierMessageBase(T parameter) {
            // identity parameter
            ParameterCollection.Add(parameter);
        }
    }
}
