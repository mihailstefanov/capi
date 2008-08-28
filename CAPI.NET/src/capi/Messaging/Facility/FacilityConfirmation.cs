namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Facility, SubCommand.Confirmation)]
    public class FacilityConfirmation : ConformationMessageBase<PLCIParameter> {
        public FacilityConfirmation()
            : base(new PLCIParameter()) {
            // Facility selector
            ParameterCollection.Add(new Parameter<short>());
            // Facility selector
            ParameterCollection.Add(new Parameter<short>());
        }

        public FacilitySelector FacilitySelector {
            get { return (FacilitySelector)((Parameter<short>)ParameterCollection[2]).Value; }
        }

        public DTMFInformation DTMFInformation {
            get {
                if (FacilitySelector != FacilitySelector.DTMF) throw new NotSupportedException();
                return (DTMFInformation)((Parameter<short>)ParameterCollection[3]).Value;
            }
        }

        internal override void Notify(CapiApplication application, MessageAsyncResult result) {
            Connection c = (Connection)result.Caller;
            c.FacilityConfirmation(this, result);
        }
    }
}
