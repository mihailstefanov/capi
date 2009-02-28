using System;
using System.Collections.Generic;
using System.Text;

namespace Mommosoft.Capi {
    public abstract class ConformationMessageBase<T> : IdentifierMessageBase<T> where T : Parameter<uint> {
        private int _infoIndex = 1;

        public ConformationMessageBase(T parameter)
            : base(parameter) {
            ParameterCollection.Add(new Parameter<short>());
        }

        public ConformationMessageBase(T parameter, int infoIndex)
            : this(parameter) {
            _infoIndex = infoIndex;
        }

        public T Identifier {
            get { return (T)ParameterCollection[0]; }
        }

        public Info Info {
            get { return ((Info)((Parameter<short>)ParameterCollection[_infoIndex]).Value); }
        }

        public bool Succeeded {
            get { return Info == Info.Success; }
        }

        internal abstract void Notify(CapiApplication application, MessageAsyncResult result);

        internal override void Notify(CapiApplication application) {
            MessageAsyncResult result = application.GetMessageAsyncResult(Number);
            Notify(application, result);
            application.RemoveMessageAsyncResult(Number);
        }
    }
}
