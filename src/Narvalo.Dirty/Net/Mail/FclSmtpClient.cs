namespace Narvalo.Mail
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Threading.Tasks;

    public class FclSmtpClient : ISmtpClient
    {
        #region Fields

        public const int DefaultTimeout = 5000;

        readonly string _password;
        readonly string _serverAddress;
        [ContractPublicPropertyName("ServerPort")]
        readonly int _serverPort;
        readonly string _userName;
        readonly bool _useSsl;

        bool _disposed = false;
        SmtpClient _smtpClient;
        [ContractPublicPropertyName("Timeout")]
        int _timeout = DefaultTimeout;

        #endregion

        #region Ctor

        public FclSmtpClient(string address, int port, string userName, string password, bool useSsl)
        {
            Contract.Requires(port >= 0);
            Contract.Requires(port <= 0xffff);

            _password = password;
            _serverAddress = address;
            _serverPort = port;
            _userName = userName;
            _useSsl = useSsl;
        }

        #endregion

        //[ContractInvariantMethod]
        //void ObjectInvariant() {
        //    Contract.Invariant(_timeout >= 0);
        //    Contract.Invariant(_serverPort >= 0);
        //    Contract.Invariant(_serverPort <= 0xffff);
        //}

        #region ISmtpClient

        public event EventHandler<SendCompletedEventArgs> SendCompletedEventHandler;

        public bool IsConnected
        {
            get
            {
                return _smtpClient != null && _smtpClient.ServicePoint.CurrentConnections > 0;
            }
        }

        public string ServerAddress { get { return _serverAddress; } }

        public int ServerPort { get { return _serverPort; } }

        public int Timeout { get { return _timeout; } set { _timeout = value; } }

        public string UserName { get { return _userName; } }

        public bool UseSsl { get { return _useSsl; } }

        public void Connect()
        {
            if (IsConnected) {
                throw new InvalidOperationException("You are already connected.");
            }

            _smtpClient = new SmtpClient(ServerAddress, ServerPort);
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new NetworkCredential(UserName, _password);
            if (UseSsl) {
                _smtpClient.EnableSsl = true;
            }
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            _smtpClient.Timeout = Timeout;
        }

        public void Disconnect()
        {
            if (IsConnected) {
                return;
            }

            // Dispose send a QUIT command to the server.
            // See http://msdn.microsoft.com/en-us/library/ee706941.aspx
            _smtpClient.Dispose();
            _smtpClient = null;
        }

        public void Send(MailObject mail)
        {
            ThrowIfNotReadyState();

            _smtpClient.Send(ToMailMessage_(mail));

            OnSendCompleted(new SendCompletedEventArgs(mail));
        }

        public void SendAsync(MailObject mail, object userState)
        {
            ThrowIfNotReadyState();

            SendCompletedEventHandler callback = delegate(object sender, AsyncCompletedEventArgs e)
            {
                if (!e.Cancelled) {
                    OnSendCompleted(new SendCompletedEventArgs(mail));
                }
            };

            _smtpClient.SendCompleted += callback;

            _smtpClient.SendAsync(ToMailMessage_(mail), userState);
        }

        public Task SendAsync(MailObject mail)
        {
            throw new NotImplementedException();

            //    ThrowIfNotReadyState();

            //    var message = mail.ToMailMessage();

            //    // Create a TaskCompletionSource to represent the operation 
            //    var tcs = new TaskCompletionSource<object>();

            //    // Register a handler that will transfer completion results to the TCS Task 
            //    SendCompletedEventHandler handler = null;
            //    handler = (sender, e) => ParallelUtil.HandleCompletion(tcs, e, () => null, () => _smtpClient.SendCompleted -= handler);
            //    _smtpClient.SendCompleted += handler;

            //    // Try to start the async operation.  If starting it fails (due to parameter validation) 
            //    // unregister the handler before allowing the exception to propagate. 
            //    try {
            //        _smtpClient.SendAsync(message, tcs);
            //    }
            //    catch (Exception ex) {
            //        _smtpClient.SendCompleted -= handler;
            //        tcs.TrySetException(ex);
            //    }

            //    // Return the task to represent the asynchronous operation 
            //    return tcs.Task;
        }

        public void SendAsyncCancel()
        {
            ThrowIfNotReadyState();

            _smtpClient.SendAsyncCancel();
        }

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
                    if (_smtpClient != null) {
                        _smtpClient.Dispose();
                    }
                }

                _smtpClient = null;
                _disposed = true;
            }
        }

        protected virtual void OnSendCompleted(SendCompletedEventArgs e)
        {
            EventHandler<SendCompletedEventArgs> localHandler = SendCompletedEventHandler;

            if (localHandler != null) {
                localHandler(this, e);
            }
        }

        protected void ThrowIfNotReadyState()
        {
            if (_disposed) {
                throw new ObjectDisposedException("FclSmtpClient");
            }
            if (!IsConnected) {
                throw new SmtpClientException("You are not currently connected.");
            }
        }

        #endregion

        static MailMessage ToMailMessage_(MailObject mail)
        {
            Contract.Requires(mail != null);

            MailMessage message = null;
            MailMessage tmpMessage = null;

            try {
                tmpMessage = new MailMessage();
                tmpMessage.From = mail.Sender;
                tmpMessage.Subject = mail.Subject;
                tmpMessage.SubjectEncoding = mail.SubjectEncoding;

                if (mail.ReturnPath != null) {
                    tmpMessage.Sender = mail.ReturnPath;
                }

                foreach (var address in mail.Recipients) {
                    tmpMessage.To.Add(address);
                }
                foreach (var address in mail.ReplyToList) {
                    tmpMessage.ReplyToList.Add(address);
                }
                foreach (var address in mail.CarbonCopyList) {
                    tmpMessage.CC.Add(address);
                }
                foreach (var address in mail.BlackCarbonCopyList) {
                    tmpMessage.Bcc.Add(address);
                }

                tmpMessage.HeadersEncoding = mail.HeadersEncoding;
                tmpMessage.Headers.Clear();
                if (mail.Headers != null) {
                    tmpMessage.Headers.Add(mail.Headers);
                }

                foreach (var attachment in mail.Attachments) {
                    tmpMessage.Attachments.Add(attachment);
                }

                if (mail.IsBodyHtml) {
                    // WARNING: don't use tmpMessage.Body or Yahoo! won't understand it
                    tmpMessage.IsBodyHtml = true;

                    using (AlternateView textView
                        = AlternateView.CreateAlternateViewFromString(
                            mail.TextBody, mail.TextBodyEncoding, MediaType.Text)) {

                        textView.TransferEncoding = TransferEncoding.QuotedPrintable;
                        tmpMessage.AlternateViews.Add(textView);
                    }

                    using (AlternateView htmlView
                        = AlternateView.CreateAlternateViewFromString(
                            mail.HtmlBody, mail.HtmlBodyEncoding, MediaType.Html)) {

                        htmlView.TransferEncoding = TransferEncoding.QuotedPrintable;
                        tmpMessage.AlternateViews.Add(htmlView);
                    }
                }
                else {
                    tmpMessage.Body = mail.TextBody;
                    tmpMessage.BodyEncoding = mail.TextBodyEncoding;
                }

                message = tmpMessage;
                tmpMessage = null;
            }
            finally {
                if (tmpMessage != null) {
                    tmpMessage.Dispose();
                }
            }

            return message;
        }
    }
}
