namespace Narvalo.Mail
{
    using System;
    using System.Collections.ObjectModel;
    using System.Net.Mail;

    public class AttachmentCollection : Collection<Attachment>, IDisposable
    {
        #region Fields

        bool _disposed = false;

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected methods

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    for (int i = 0; i < Count; i += 1) {
                        this[i].Dispose();
                    }
                }

                _disposed = true;
            }
        }

        #endregion
    }
}
