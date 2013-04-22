namespace Pacr.WebSite.HttpHandlers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Mail;
    using Pacr.Infrastructure.Addressing;

    [Serializable]
    public class LogOnQuery : LogOnOptions
    {
        public static readonly string EmailAddressKey = "email";
        public static readonly string PasswordKey = "password";

        MailAddress _emailAddress;
        string _password;

        [Required(ErrorMessage = "EmailAddress is required.")]
        public MailAddress EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}
