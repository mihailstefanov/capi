namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MessageIdentityComparer : IEqualityComparer<MessageIdentity> {
        bool IEqualityComparer<MessageIdentity>.Equals(MessageIdentity x, MessageIdentity y) {
            return x.Equals(y);

        }

        int IEqualityComparer<MessageIdentity>.GetHashCode(MessageIdentity obj) {
            return obj.Identity.GetHashCode();
        }
    }

    public struct MessageIdentity : IEquatable<MessageIdentity> {
        private readonly Command _command;
        private readonly SubCommand _subcommand;
        private readonly short _identity;

        public MessageIdentity(Command command, SubCommand subcommand) {
            _command = command;
            _subcommand = subcommand;
            _identity = GetIdentity(command, subcommand);
        }

        public static short GetIdentity(Command command, SubCommand subcommand) {
            return (short)((byte)command + (short)((byte)subcommand << 8));
        }

        public static Command GetCommand(short identity) {
            return (Command)identity;
        }

        public Command Command {
            get { return _command; }
        }

        public SubCommand SubCommand {
            get { return _subcommand; }
        }

        public short Identity {
            get { return _identity; }
        }

        public bool Equals(MessageIdentity other) {
            return _identity == other._identity;
        }
    }
}
