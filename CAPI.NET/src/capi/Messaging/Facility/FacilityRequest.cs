namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Facility, SubCommand.Request)]
    public class FacilityRequest : RequestMessageBase<Parameter<uint>> {
        public FacilityRequest()
            : base(new Parameter<uint>()) {
            // Facility selector
            ParameterCollection.Add(new Parameter<short>());
            // Facility Request Parameter 
            ParameterCollection.Add(new DTMFFacilityRequestParameter());
        }

        public FacilitySelector FacilitySelector {
            get { return (FacilitySelector)((Parameter<short>)ParameterCollection[1]).Value; }
            set { ((Parameter<short>)ParameterCollection[1]).Value = (short)value; }
        }

        public DTMFFacilityRequestParameter DTMFFacilityRequestParameter {
            get { return (DTMFFacilityRequestParameter)ParameterCollection[2]; }
            set { ParameterCollection[2] = value; }
        }
    }
}
