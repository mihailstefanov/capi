using System;
using System.Collections.Generic;
using System.Text;

namespace Mommosoft.Capi {
    public abstract class IndicationMessageBase<T> : IdentifierMessageBase<T> where T : Parameter<int> {
        protected IndicationMessageBase(T parameter) : base(parameter) { }

        public T Identifier {
            get { return (T)ParameterCollection[0]; }
        }
    }
}
