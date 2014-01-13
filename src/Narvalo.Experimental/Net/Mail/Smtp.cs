namespace Narvalo.Mail {
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;

    /// <summary>
    /// 
    /// </summary>
    /// http://fr.wikipedia.org/wiki/Simple_Mail_Transfer_Protocol
    /// http://tools.ietf.org/html/rfc5321
    public class Smtp : TcpClient {
        public const int DefaultPort = 25;

        private static String[] Commands = {
            "HELO",             // 0
            "MAIL FROM:",       // 1
            "RCPT TO:",         // 2
            "DATA",             // 3
            "SEND FROM:",       // 4
            "SOML FROM:",       // 5
            "SAML FROM:",       // 6
            "RSET",             // 7
            "VRFY",             // 8
            "EXPN",             // 9
            "HELP",             // 10
            "NOOP",             // 11
            "TURN",             // 12
            "QUIT",             // 13
            "AUTH",             // 14
            "EHLO"              // 15
        };

        public Smtp() : base() { }

        //public Smtp(AddressFamily family) : base(AddressFamily.InterNetwork) { }

        public Smtp(string hostname) : base(hostname, DefaultPort) { }

        public Smtp(string hostname, int port) : base(hostname, port) { }

        public int LastReplyCode {
            get {
                throw new NotImplementedException();
            }
        }

        public string LastReplyString {
            get {
                throw new NotImplementedException();
            }
        }

        public IList<string> LastReplyStrings {
            get {
                throw new NotImplementedException();
            }
        }

        public int GetReply() {
            throw new NotImplementedException();
        }

        public void Disconnect() {
            throw new NotImplementedException();
        }

        public int SendCommand(int command) {
            throw new NotImplementedException();
        }

        public int SendCommand(string command) {
            throw new NotImplementedException();
        }

        public int SendCommand(int command, string args) {
            throw new NotImplementedException();
        }

        public int SendCommand(string command, string args) {
            throw new NotImplementedException();
        }

        public int Helo(string hostname) {
            throw new NotImplementedException();
        }

        public int Mail(string reversePath) {
            throw new NotImplementedException();
        }

        public int Rcpt(string forwardPath) {
            throw new NotImplementedException();
        }

        public int Data() {
            throw new NotImplementedException();
        }

        public int Send(string reversePath) {
            throw new NotImplementedException();
        }

        public int Saml(string reversePath) {
            throw new NotImplementedException();
        }

        public int Soml(string reversePath) {
            throw new NotImplementedException();
        }

        public int Rset() {
            throw new NotImplementedException();
        }

        public int Vrfy(string user) {
            throw new NotImplementedException();
        }

        public int Expn(string user) {
            throw new NotImplementedException();
        }

        public int Help() {
            throw new NotImplementedException();
        }

        public int Help(string command) {
            throw new NotImplementedException();
        }

        public int Noop() {
            throw new NotImplementedException();
        }

        public int Turn() {
            throw new NotImplementedException();
        }

        public int Quit() {
            throw new NotImplementedException();
        }

        #region Commands

        private static String GetCommand(int command) {
            return Commands[command];
        }

        private static String GetCommand(SmtpCommand command) {
            return Commands[(int)command];
        }

        #endregion
    }
}
