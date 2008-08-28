namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.DataB3, SubCommand.Confirmation)]
    public class DataB3Confirmation : ConformationMessageBase<NCCIParameter> {
        public DataB3Confirmation()
            : base(new NCCIParameter()) {
            // Data Handle.
            ParameterCollection.Add(new Parameter<short>());

            ParameterCollection.Add(new Parameter<short>());
        }

        public short DataHandle {
            get { return ((Parameter<short>)ParameterCollection[1]).Value; }
        }

        internal override void Notify(CapiApplication application, MessageAsyncResult result) {
            throw new NotImplementedException();
        }
    }
}
