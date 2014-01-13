namespace Narvalo.Playground
{
    using System;
    using System.Web;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;
    using Narvalo.Net.Mail;

    public class LogOnQueryBinder
    {
        public LogOnQueryBinder() { }

        public Outcome<LogOnQuery> Bind(HttpRequest request)
        {
            var form = request.Form;

            // > Paramètres obligatoires <

            var emailAddress = form.ParseValue(
                LogOnQuery.EmailAddressKey, MailAddressUtility.Create, () => "XXX");
            if (emailAddress.Unsuccessful) { return emailAddress.Forget<LogOnQuery>(); }

            var password = form.MayGetValue(LogOnQuery.PasswordKey);
            if (password.IsNone) { return Failure_("XXX"); }

            // > Paramètres optionnels <

            var targetUrl = form.ParseSomeValue(
                LogOnQuery.TargetUrlKey, _ => MayParse.ToUri(_, UriKind.Relative));
            if (targetUrl.Unsuccessful) { return Failure_("XXX"); }

            var persistent = form.ParseSomeValue(
                LogOnQuery.CreatePersistentCookieKey, _ => MayParse.ToBoolean(_, BooleanStyles.Any));
            if (persistent.Unsuccessful) { return Failure_("XXX"); }

            // > Création du modèle <

            var model = new LogOnQuery {
                EmailAddress = emailAddress.Value,
                Password = password.Value
            };
            targetUrl.Value.WhenSome(_ => { model.TargetUrl = _; });
            persistent.Value.WhenSome(_ => { model.CreatePersistentCookie = _; });

            return Outcome.Success(model);
        }

        static Outcome<LogOnQuery> Failure_(string resourceKey)
        {
            return Outcome.Failure<LogOnQuery>(resourceKey);
        }
    }
}
