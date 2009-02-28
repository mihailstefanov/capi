namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Facility, SubCommand.Indication)]
    public class FacilityIndication : IndicationMessageBase<Parameter<uint>> {
        public FacilityIndication()
            : base(new Parameter<uint>()) {
            // Facility selector
            ParameterCollection.Add(new Parameter<short>());

            /////////////////////////////////////////////////
            ParameterCollection.Add(new Parameter<string>());
        }

        public FacilitySelector FacilitySelector {
            get { return (FacilitySelector)((Parameter<short>)ParameterCollection[1]).Value; }
        }

        /// <summary>
        /// Received characters, coded as IA5-char. '0' to '9', '*', '#', 'A',
        /// 'B', 'C' or 'D'; or
        /// ‘X’: Recognition of fax tone CNG (1.1 kHz)
        /// ‘Y’: Recognition of fax tone CED (2.1 kHz)
        /// </summary>
        public string Digits {
            get { return ((Parameter<string>)ParameterCollection[2]).Value; }
        }

        internal override void Notify(CapiApplication application) {
            PLCIParameter p = new PLCIParameter(Identifier.Value);
            Controller controller = application.GetControllerByID(p.ControllerID);
            Connection connection = controller.GetConnectionByPLCI(p.PLCI);
            connection.FacilityIndication(this);
        }
    }
}
