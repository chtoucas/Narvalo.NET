namespace Narvalo.Mail
{
    using System;

    public class SendCompletedEventArgs : EventArgs
    {
        readonly MailObject _mail;

        public SendCompletedEventArgs(MailObject mail)
        {
            _mail = mail;
        }

        public MailObject Mail
        {
            get
            {
                return _mail;
            }
        }
    }
}
