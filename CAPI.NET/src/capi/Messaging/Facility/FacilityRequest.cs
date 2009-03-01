namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Facility, SubCommand.Request)]
    public class FacilityRequest : RequestMessageBase<Parameter<uint>> {
        public FacilityRequest(IParameter requestParmaeter)
            : base(new Parameter<uint>()) {
            // Facility selector
            ParameterCollection.Add(new Parameter<short>());
            // Facility Request Parameter 
            ParameterCollection.Add(requestParmaeter);
        }

        public FacilitySelector FacilitySelector {
            get { return (FacilitySelector)((Parameter<short>)ParameterCollection[1]).Value; }
            set { ((Parameter<short>)ParameterCollection[1]).Value = (short)value; }
        }

        public IParameter FacilityRequestParameter {
            get { return ParameterCollection[2]; }
            set { ParameterCollection[2] = value; }
        }
    }
}
