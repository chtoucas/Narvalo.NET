namespace Narvalo.Mail {
    public enum SmtpCommand {
        // Machine codes.
        Helo = 0,
        Ehlo = 15,
        Mail = 1,
        Rcpt = 2,
        Data = 3,
        Rset = 7,
        Noop = 11,
        Quit = 13,
        Vrfy = 8,

        Send = 4,
        Soml = 5,
        Saml = 6,
        Expn = 9,
        Help = 10,
        Turn = 12,
        Auth = 14,

        // Human-readable commands.
        Hello = Helo,
        LogOn = Helo,
        MailFrom = Mail,
        Recipient = Rcpt,
        SendMessageData = Data,
        SendFrom = Send,
        SendOrMailFrom = Soml,
        SendAndMailFrom = Saml,
        Reset = Rset,
        Verify = Vrfy,
        Expand = Expn,
        LogOff = Quit,
    }
}
