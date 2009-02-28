using System;
using System.Collections.Generic;
using System.Text;

namespace Mommosoft.Capi {
    public class ResponseMessageBase<T> : IdentifierMessageBase<T> where T : Parameter<uint> {
        protected ResponseMessageBase(T parameter, IndicationMessageBase<T> indication)
            : base(parameter) {
            Number = indication.Number;
            Identifier.Value = indication.Identifier.Value;
        }

        public T Identifier {
            get { return (T)ParameterCollection[0]; }
            set { ParameterCollection[0] = value; }
        }
    }
}
