namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.ConnectActive, SubCommand.Indication)]
    public class ConnectActiveIndication : IndicationMessageBase<PLCIParameter> {
        public ConnectActiveIndication()
            : base(new PLCIParameter()) {
            // Connected number
            ParameterCollection.Add(new PrefixedStringParameter(new byte[] { 0, 128 }));

            // Connected subaddress
            ParameterCollection.Add(new PrefixedStringParameter(new byte[] { 128 }));

            // LLC
            ParameterCollection.Add(new Parameter<byte>());
        }

        /// <summary>
        /// Gets connected number.
        /// </summary>
        public string ConnectedNumber {
            get { return ((Parameter<string>)ParameterCollection[1]).Value; }
        }

        /// <summary>
        /// Gets connected subaddress
        /// </summary>
        public string ConnectedSubaddress {
            get { return ((Parameter<string>)ParameterCollection[2]).Value; }
        }

        internal override void Notify(CapiApplication application) {
            Controller controller = application.GetControllerByID(Identifier.ControllerID);
            Connection connection = controller.GetConnectionByPLCI(Identifier.PLCI);
            connection.ConnectActiveIndication(this);
        }
    }
}
