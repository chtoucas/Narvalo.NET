namespace Narvalo.Mail {
    public interface IImapClient {
        bool IsConnected { get; }
        string ServerAddress { get; }
        int ServerPort { get; }
        int Timeout { get; set; }
        string UserName { get; }
        bool UseSsl { get; }

        void Connect();
        int CountMessages();
        //void Delete(int id);
        //void Delete(IList<int> ids);
        void Disconnect();
        //Dictionary<int, IEmailMessage> FetchNotifications();
        //IList<IEmailMessage> FetchHeaders();
        //IList<IEmailMessage> FetchMessages();
    }
}
