namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [MessageIdentity(Command.Listen, SubCommand.Confirmation)]
    public class ListenConfirmation : ConformationMessageBase<ControllerParameter> {
        public ListenConfirmation() : base(new ControllerParameter()) { }

        internal override void Notify(CapiApplication application, MessageAsyncResult result) {
            Controller c = (Controller)result.Caller;
            c.ListenConfirmation(this, result);
        }
    }
}
