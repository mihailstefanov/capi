namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Facility, SubCommand.Confirmation)]
    public class FacilityConfirmation : ConformationMessageBase<Parameter<uint>> {
        public FacilityConfirmation()
            : base(new Parameter<uint>()) {
            // Facility selector
            ParameterCollection.Add(new Parameter<short>());
            // Facility confirmation parameter
            ParameterCollection.Add(new StructParameter());
        }

        public FacilitySelector FacilitySelector {
            get { return (FacilitySelector)((Parameter<short>)ParameterCollection[2]).Value; }
        }

        public StructParameter Confirmation {
            get {
                return ((Parameter<StructParameter>)ParameterCollection[3]).Value;
            }
        }

        internal override void Notify(CapiApplication application, MessageAsyncResult result) {
            Connection connection = result.Caller as Connection;
            if (connection != null) {
                connection.FacilityConfirmation(this, result);
            }
        }
    }
}
