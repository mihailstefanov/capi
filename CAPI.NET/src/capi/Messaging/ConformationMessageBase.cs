using System;
using System.Collections.Generic;
using System.Text;

namespace Mommosoft.Capi {
    public abstract class ConformationMessageBase<T> : IdentifierMessageBase<T> where T : Parameter<uint> {
        private int _infoIndex = 1;

        public ConformationMessageBase(T parameter)
            : base(parameter) {
            ParameterCollection.Add(new Parameter<ushort>());
        }

        public ConformationMessageBase(T parameter, int infoIndex)
            : base(parameter) {
            _infoIndex = infoIndex;
        }

        public T Identifier {
            get { return (T)ParameterCollection[0]; }
        }

        public Info Info {
            get { return ((Info)((Parameter<ushort>)ParameterCollection[_infoIndex]).Value); }
        }

        public bool Succeeded {
            // Info's with value 0x00xx are only warnings, 
            // the corresponding requests have been processed.
            get { return (ushort)Info < 0x00FF; }
        }

        internal abstract void Notify(CapiApplication application, MessageAsyncResult result);

        internal override void Notify(CapiApplication application) {
            MessageAsyncResult result = application.GetMessageAsyncResult(Number);
            if (result != null) {
                Notify(application, result);
                application.RemoveMessageAsyncResult(Number);
            }
        }
    }
}
