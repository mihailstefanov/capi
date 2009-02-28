namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class RequestMessageBase<T> : IdentifierMessageBase<T> where T : Parameter<uint> {
        protected RequestMessageBase(T parameter) : base(parameter) { }

        public T Identifier {
            get { return (T)ParameterCollection[0]; }
            set { ParameterCollection[0] = value; }
        }
    }
}
