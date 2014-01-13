namespace Narvalo.Mail.Fcl {
    using System.Diagnostics.Contracts;
    using System.Net.Mail;
    using System.Net.Mime;

    internal static class MailObjectExtensions {
        public static MailMessage ToMailMessage(this MailObject mail) {
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
                            mail.TextBody, mail.TextBodyEncoding, Constants.TextContentType)) {

                        textView.TransferEncoding = TransferEncoding.QuotedPrintable;
                        tmpMessage.AlternateViews.Add(textView);
                    }

                    using (AlternateView htmlView 
                        = AlternateView.CreateAlternateViewFromString(
                            mail.HtmlBody, mail.HtmlBodyEncoding, Constants.HtmlContentType)) {

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
