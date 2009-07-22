namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.DataB3, SubCommand.Indication)]
    public class DataB3Indication : IndicationMessageBase<NCCIParameter> {
        public DataB3Indication()
            : base(new NCCIParameter()) {
            // Data
            ParameterCollection.Add(new Parameter<int>());
            // Data Length
            ParameterCollection.Add(new Parameter<short>());
            // Data Handle
            ParameterCollection.Add(new Parameter<short>());
        }


        public short Handle {
            get { return ((Parameter<short>)ParameterCollection[3]).Value; }
        }

        internal override void Notify(CapiApplication application) {
            Controller ccontroller = application.GetControllerByID(Identifier.ControllerID);
            Connection connection = ccontroller.GetConnectionByPLCI(Identifier.PLCI);
            connection.DataB3Indication(this);
        }
    }
}
