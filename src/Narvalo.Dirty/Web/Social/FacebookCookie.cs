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
    using Narvalo.Linq;

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

        public static FacebookCookie? Create(
            HttpContextBase httpContext,
            string appId,
            string appSecret)
        {
            Require.NotNull(httpContext, "httpContext");
            Require.NotNullOrEmpty(appId, "appId");
            Require.NotNullOrEmpty(appSecret, "appSecret");

            return (from _ in MayGetHttpCookie_(httpContext, appId) select ParseHttpCookie_(_, appSecret))
                .ValueOrDefault();
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

        static Maybe<HttpCookie> MayGetHttpCookie_(HttpContextBase httpContext, string appId)
        {
            HttpRequestBase request = httpContext.Request;
            if (request == null || request.Cookies == null) {
                return Maybe<HttpCookie>.None;
            }

            string cookieName = Format.InvariantCulture(CookieNameFormat_, appId);

            return Maybe.Create(request.Cookies[cookieName]);
        }

        static FacebookCookie? ParseHttpCookie_(HttpCookie httpCookie, string appSecret)
        {
            NameValueCollection nvc = GetArguments_(httpCookie);

            if (!CheckSignature_(nvc, appSecret)) {
                throw new SecurityException("Invalid signature found for facebook cookie.");
            }

            var expiresOn = ParseTo.NullableDateTime(nvc[ExpiresOnKey_]);
            if (expiresOn.HasValue) { return null; }

            var userId = ParseTo.NullableInt64(nvc[UserIdKey_]);
            if (userId.HasValue) { return null; }

            return new FacebookCookie(
                nvc[AccessTokenKey_],
                expiresOn.Value,
                nvc[SecretKey_],
                nvc[SessionKeyKey_],
                userId.Value
            );
        }

        #endregion
    }
}
