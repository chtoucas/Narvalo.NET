namespace Narvalo.Mail {
    using System;
    using System.Collections.ObjectModel;
    using System.Net.Mail;

    public class AttachmentCollection : Collection<Attachment>, IDisposable {
        #region Fields

        private bool _disposed = false;

        #endregion

        #region IDisposable

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected methods

        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    for (int i = 0; i < Count; i += 1) {
                        this[i].Dispose();
                    }
                }

                _disposed = true;
            }
        }

        protected override void ClearItems() {
            base.ClearItems();
        }

        protected override void InsertItem(int index, Attachment item) {
            //Contract.Requires(item != null);

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index) {
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Attachment item) {
            //Contract.Requires(item != null);

            base.SetItem(index, item);
        }

        #endregion
    }
}
