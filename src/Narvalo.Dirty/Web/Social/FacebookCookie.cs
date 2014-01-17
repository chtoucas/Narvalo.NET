namespace Narvalo.Web.Social
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using Narvalo;
    using Narvalo.Fx;

    public struct FacebookCookie
    {
        // Le format est : "fbs_{appId}".
        const string CookieNameFormat_ = @"fbs_{0}";

        const string AccessTokenKey_ = "access_token";
        const string ExpiresOnKey_ = "expires";
        const string SecretKey_ = "secret";
        const string SessionKeyKey_ = "session_key";
        const string SignatureKey_ = "sig";
        const string UserIdKey_ = "uid";

        string _accessToken;
        DateTime _expiresOn;
        string _secret;
        string _sessionKey;
        long _userId;

        public FacebookCookie(
            string accessToken,
            DateTime expiresOn,
            string secret,
            string sessionKey,
            long userId)
        {
            _accessToken = accessToken;
            _expiresOn = expiresOn;
            _secret = secret;
            _sessionKey = sessionKey;
            _userId = userId;
        }

        public string AccessToken { get { return _accessToken; } }
        public DateTime ExpiresOn { get { return _expiresOn; } }
        public string Secret { get { return _secret; } }
        public string SessionKey { get { return _sessionKey; } }
        public long UserId { get { return _userId; } }

        public static Maybe<FacebookCookie> MayGet(
            HttpContextBase httpContext,
            string appId,
            string appSecret)
        {
            Require.NotNull(httpContext, "httpContext");
            Require.NotNullOrEmpty(appId, "appId");
            Require.NotNullOrEmpty(appSecret, "appSecret");

            return MayGetHttpCookie_(httpContext, appId)
                .Bind(_ => MayParseHttpCookie_(_, appSecret));
        }

        public static bool TryGet(
            HttpContextBase httpContext,
            string appId,
            string appSecret,
            out FacebookCookie cookie)
        {
            return MayGet(httpContext, appId, appSecret).TrySet(out cookie);
        }

        #region Membres privés

        static bool CheckSignature_(NameValueCollection nvc, string appSecret)
        {
            string[] keys = nvc.AllKeys;
            Array.Sort(keys);

            var payload = new StringBuilder();
            foreach (string key in keys) {
                if (key.Equals(SignatureKey_, StringComparison.OrdinalIgnoreCase)) {
                    continue;
                }
                payload.AppendFormat(@"{0}={1}", key, nvc[key]);
            }
            payload.Append(appSecret);

            var signature = new StringBuilder();
            using (MD5 md5 = MD5CryptoServiceProvider.Create()) {
                byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(payload.ToString()));

                for (int i = 0; i < hash.Length; i++) {
                    signature.Append(hash[i].ToString("X2", CultureInfo.InvariantCulture));
                }
            }

            return String.Equals(
                nvc[SignatureKey_],
                signature.ToString(),
                StringComparison.OrdinalIgnoreCase);
        }

        static NameValueCollection GetArguments_(HttpCookie httpCookie)
        {
            return HttpUtility.ParseQueryString(httpCookie.Value.Replace("\"", String.Empty));
        }

        static string GetHttpCookieName_(string appId)
        {
            return String.Format(CultureInfo.InvariantCulture, CookieNameFormat_, appId);
        }

        static Maybe<HttpCookie> MayGetHttpCookie_(HttpContextBase httpContext, string appId)
        {
            HttpRequestBase request = httpContext.Request;
            if (request == null || request.Cookies == null) {
                return Maybe<HttpCookie>.None;
            }

            string cookieName = GetHttpCookieName_(appId);

            return Maybe.Create(request.Cookies[cookieName]);
        }

        static Maybe<FacebookCookie> MayParseHttpCookie_(HttpCookie httpCookie, string appSecret)
        {
            NameValueCollection nvc = GetArguments_(httpCookie);

            if (!CheckSignature_(nvc, appSecret)) {
                throw new SecurityException("Invalid signature found for facebook cookie.");
            }

            var expiresOn = MayParse.ToDateTime(nvc[ExpiresOnKey_]);
            if (expiresOn.IsNone) { return Maybe<FacebookCookie>.None; }

            var userId = MayParse.ToInt64(nvc[UserIdKey_]);
            if (userId.IsNone) { return Maybe<FacebookCookie>.None; }

            var cookie = new FacebookCookie(
                nvc[AccessTokenKey_],
                expiresOn.Value,
                nvc[SecretKey_],
                nvc[SessionKeyKey_],
                userId.Value
            );

            return Maybe.Create(cookie);
        }

        #endregion
    }
}
