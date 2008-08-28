namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class MessageIdentityDictionary : Dictionary<MessageIdentity, Type> {
        public MessageIdentityDictionary()
            : base(new MessageIdentityComparer()) {
        }
        public Type GetValue(Command command, SubCommand subcommand) {
            Type type;
            TryGetValue(new MessageIdentity(command, subcommand), out type);
            return type;
        }
    }
}
