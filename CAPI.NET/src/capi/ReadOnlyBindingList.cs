namespace Mommosoft.Capi {
    using System;
    using System.ComponentModel;

    [Serializable]
    public abstract class ReadOnlyBindingList<C> : BindingList<C> {
        private bool _isReadOnly = true;

        /// <summary>
        /// Gets or sets a value indicating whether the list is readonly.
        /// </summary>
        /// <remarks>
        /// Subclasses can set this value to unlock the collection
        /// in order to alter the collection's data.
        /// </remarks>
        /// <value>True indicates that the list is readonly.</value>
        public bool IsReadOnly {
            get { return _isReadOnly; }
            protected set { _isReadOnly = value; }
        }

        protected ReadOnlyBindingList() {
            AllowEdit = false;
            AllowRemove = false;
            AllowNew = false;
        }

        /// <summary>
        /// Prevents clearing the collection.
        /// </summary>
        protected override void ClearItems() {
            if (!IsReadOnly) {
                bool oldValue = AllowRemove;
                AllowRemove = true;
                base.ClearItems();
                AllowRemove = oldValue;
            } else
                throw Error.NotSupported();
        }

        /// <summary>
        /// Prevents insertion of items into the collection.
        /// </summary>
        protected override object AddNewCore() {
            if (!IsReadOnly)
                return base.AddNewCore();
            else
                throw Error.NotSupported();
        }

        /// <summary>
        /// Prevents insertion of items into the collection.
        /// </summary>
        protected override void InsertItem(int index, C item) {
            if (!IsReadOnly)
                ProtectedInsertItem(index, item);
            else
                throw Error.NotSupported();
        }

        protected void ProtectedInsertItem(int index, C item) {
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Removes the item at the specified index if the collection is
        /// not in readonly mode.
        /// </summary>
        protected override void RemoveItem(int index) {
            if (!IsReadOnly) {
                ProtectedRemoveItem(index);
            } else
                throw Error.NotSupported();
        }

        protected void ProtectedRemoveItem(int index) {
            bool oldValue = AllowRemove;
            AllowRemove = true;
            base.RemoveItem(index);
            AllowRemove = oldValue;
        }

        /// <summary>
        /// Replaces the item at the specified index with the 
        /// specified item if the collection is not in
        /// readonly mode.
        /// </summary>
        protected override void SetItem(int index, C item) {
            if (!IsReadOnly)
                ProtectedSetItem(index, item);
            else
                throw Error.NotSupported();
        }

        protected void ProtectedSetItem(int index, C item) {
            base.SetItem(index, item);
        }
    }
}
