namespace Narvalo.Mail {
    using System;

    public struct SmtpClientSettings : IEquatable<SmtpClientSettings> {
        private readonly string _host;
        private readonly string _password;
        private readonly int _port;
        private readonly string _userName;
        private readonly bool _useSsl;

        //public SmtpClientSettings(SmtpConfiguration config) {
        //    if (config == null) {
        //        throw new ArgumentNullException("config");
        //    }

        //    _host = config.Host;
        //    _password = config.Password;
        //    _port = config.Port;
        //    _userName = config.UserName;
        //    _useSsl = config.UseSsl;
        //}

        public SmtpClientSettings(string host, int port, string userName, string password, bool useSsl) {
            _host = host;
            _password = password;
            _port = port;
            _userName = userName;
            _useSsl = useSsl;
        }

        public string Host { get { return _host; } }
        public string Password { get { return _password; } }
        public int Port { get { return _port; } }
        public string UserName { get { return _userName; } }
        public bool UseSsl { get { return _useSsl; } }

        #region IEquatable<SmtpSettings>

        public bool Equals(SmtpClientSettings other) {
            return _host == other._host
                && _password == other._password
                && _port == other._port
                && _userName == other._userName
                && _useSsl == other._useSsl;
        }

        #endregion

        public override int GetHashCode() {
            throw new NotImplementedException();
        }

        public static bool operator ==(SmtpClientSettings left, SmtpClientSettings right) {
            return left.Equals(right);
        }

        public static bool operator !=(SmtpClientSettings left, SmtpClientSettings right) {
            return !left.Equals(right);
        }

        public override bool Equals(object obj) {
            if (!(obj is SmtpClientSettings)) {
                return false;
            }

            return Equals((SmtpClientSettings)obj);
        }
    }
}
