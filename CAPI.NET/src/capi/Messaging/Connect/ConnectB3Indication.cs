namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.ConnectB3, SubCommand.Indication)]
    public class ConnectB3Indication : IndicationMessageBase<NCCIParameter> {
        public ConnectB3Indication()
            : base(new NCCIParameter()) {
            // NCPI
            if (IntPtr.Size == 4) {
                ParameterCollection.Add(new Parameter<int>());
            } else if (IntPtr.Size == 8) {
                ParameterCollection.Add(new Parameter<Int64>());
            }
        }

        internal override void Notify(CapiApplication application) {
            Controller ccontroller = application.GetControllerByID(Identifier.ControllerID);
            Connection connection = ccontroller.GetConnectionByPLCI(Identifier.PLCI);
            connection.ConnectB3Indication(this);
        }
    }
}
