namespace Narvalo.Mail {
    using System;
    using System.Collections.Generic;

    public interface IPop3Client : IDisposable {
        bool IsConnected { get; }
        string ServerAddress { get; }
        int ServerPort { get; }
        int Timeout { get; set; }
        string UserName { get; }
        bool UseSsl { get; }

        void Connect();
        int CountMessages();
        void Delete(int id);
        void Delete(IList<int> ids);
        void Disconnect();
        Dictionary<int, MailObject> FetchNotifications();
        IList<MailObject> FetchHeaders();
        IList<MailObject> FetchMessages();
    }
}
