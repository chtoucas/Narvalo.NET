namespace Narvalo.Mail {
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Narvalo.Diagnostics;

    [ContractClass(typeof(ISmtpClientContract))]
    public interface ISmtpClient : IDisposable {
        event EventHandler<SendCompletedEventArgs> SendCompletedEventHandler;

        bool IsConnected { get; }
        string ServerAddress { get; }
        int ServerPort { get; }
        int Timeout { get; set; }
        string UserName { get; }
        bool UseSsl { get; }

        void Connect();
        void Disconnect();
        void Send(MailObject mail);
        void SendAsync(MailObject mail, object userState);
        Task SendAsync(MailObject mail);
        void SendAsyncCancel();
    }

    [ContractClassFor(typeof(ISmtpClient))]
    internal abstract class ISmtpClientContract : ISmtpClient {
        #region ISmtpClient

        abstract public event EventHandler<SendCompletedEventArgs> SendCompletedEventHandler;

        abstract public bool IsConnected { get; }
        abstract public string ServerAddress { get; }
        abstract public string UserName { get; }
        abstract public bool UseSsl { get; }

        abstract public void Connect();
        abstract public void Disconnect();

        public int ServerPort {
            get {
                Contract.Ensures(ServerPort >= 0);
                Contract.Ensures(ServerPort <= 0xffff);
                return default(int);
            }
        }

        public int Timeout {
            get {
                Contract.Ensures(Timeout >= 0);
                return default(int);
            }
            set {
                Contract.Requires(value >= 0);
            }
        }

        [ContractInvariantMethod]
        void ObjectInvariant() {
            Contract.Invariant(Timeout >= 0);
            Contract.Invariant(ServerPort >= 0);
            Contract.Invariant(ServerPort <= 0xffff);
        }

        void ISmtpClient.Send(MailObject mail) {
            Requires.NotNull(mail, "mail");
        }

        void ISmtpClient.SendAsync(MailObject mail, object userState) {
            Requires.NotNull(mail, "mail");
        }

        Task ISmtpClient.SendAsync(MailObject mail) {
            Requires.NotNull(mail, "mail");
            return null;
        }

        abstract public void SendAsyncCancel();

        #endregion

        #region IDisposable

        abstract public void Dispose();

        #endregion
    }
}
