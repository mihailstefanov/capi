namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.DataB3, SubCommand.Confirmation)]
    public class DataB3Confirmation : ConformationMessageBase<NCCIParameter> {
        public DataB3Confirmation()
            : base(new NCCIParameter(), 2) {
            // Data Handle.
            ParameterCollection.Add(new Parameter<short>());
            // Info
            ParameterCollection.Add(new Parameter<ushort>());
        }

        public short DataHandle {
            get { return ((Parameter<short>)ParameterCollection[1]).Value; }
        }

        internal override void Notify(CapiApplication application, MessageAsyncResult result) {
            Controller controller = application.GetControllerByID(Identifier.ControllerID);
            Connection connection = controller.GetConnectionByPLCI(Identifier.PLCI);
            connection.DataB3Confirmation(this, result);
        } 
    }
}
