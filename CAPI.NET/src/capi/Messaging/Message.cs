namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Reflection;
    using System.Collections;
    using System.Diagnostics;

    public abstract class Message : ICapiSerializable {
        private ParameterCollection _parameters;
        private static MessageIdentityDictionary s_identityDictionary;
        private MessageIdentity _identity;
        private short _number;

        protected Message() {
            _parameters = new ParameterCollection();
            _identity = GetMessageIdentity(this.GetType());
        }

        internal virtual void Notify(CapiApplication application) {
        }

        internal short Number {
            get { return _number; }
            set { _number = value; }
        }

        public MessageIdentity Identity {
            get { return _identity; }
        }

        public ParameterCollection ParameterCollection {
            get { return _parameters; }
        }

        public byte Length {
            get { return _parameters.Length; }
        }

        protected virtual void Read(BinaryReader reader) {
            ((ICapiSerializable)_parameters).Read(reader);
        }

        void ICapiSerializable.Read(BinaryReader reader) {
            Read(reader);
        }

        protected virtual void Write(BinaryWriter writer) {
            ((ICapiSerializable)_parameters).Write(writer);
        }

        void ICapiSerializable.Write(BinaryWriter writer) {
            Write(writer);
        }

        internal static Type GetMessageType(Command command, SubCommand subcommand) {
            return IdentityDictionary.GetValue(command, subcommand);
        }

        private static MessageIdentityDictionary IdentityDictionary {
            get {
                MessageIdentityDictionary dictionary = s_identityDictionary;
                if (dictionary == null) {
                    dictionary = CreateDictionary();
                    s_identityDictionary = dictionary;
                }
                return dictionary;
            }
        }
        private static MessageIdentityDictionary CreateDictionary() {
            MessageIdentityDictionary dictionary = new MessageIdentityDictionary();
            Type mt = typeof(Message);
            Type[] at = Assembly.GetAssembly(mt).GetTypes();
            foreach (Type t in at) {
                if (mt.IsAssignableFrom(t)) {
                    MessageIdentity identity = GetMessageIdentity(t);
                    if (identity.Command != Command.Undefined) {
                        dictionary.Add(identity, t);
                    }
                }
            }
            return dictionary;
        }

        private static MessageIdentity GetMessageIdentity(Type t) {
            object[] a = t.GetCustomAttributes(typeof(MessageIdentityAttribute), false);
            if (a.Length != 0) {
                MessageIdentityAttribute mia = (MessageIdentityAttribute)a[0];
                return new MessageIdentity(mia.Command, mia.SubCommand);
            }
            return new MessageIdentity();
        }
    }
}
