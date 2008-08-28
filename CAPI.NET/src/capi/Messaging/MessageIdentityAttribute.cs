namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MessageIdentityAttribute : Attribute {
        private Command _command;
        private SubCommand _subcommand;

        public MessageIdentityAttribute(Command command, SubCommand subcommand) {
            _command = command;
            _subcommand = subcommand;
        }

        public Command Command {
            get { return _command; }
            set { _command = value; }
        }

        public SubCommand SubCommand {
            get { return _subcommand; }
            set { _subcommand = value; }
        }
    }
}
