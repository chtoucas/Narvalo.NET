namespace Narvalo.Web.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using Narvalo;
    using Narvalo.Linq;

    // FIXME: Ajouter une méthode statique de récupération Current.
    public class NarvaloWebSettings
    {
        const string SettingPrefix_ = "narvalo:";

        const bool DefaultEnableWhiteSpaceBusting_ = true;

        static readonly Lazy<NarvaloWebSettings> Current_ = new Lazy<NarvaloWebSettings>(Initialize_);

        bool _enableWhiteSpaceBusting = DefaultEnableWhiteSpaceBusting_;

        public static NarvaloWebSettings Current { get { return Current_.Value; } }

        public bool EnableWhiteSpaceBusting
        {
            get { return _enableWhiteSpaceBusting; }
            set { _enableWhiteSpaceBusting = value; }
        }

        public static NarvaloWebSettings FromConfiguration()
        {
            return (new NarvaloWebSettings()).Load();
        }

        public NarvaloWebSettings Load()
        {
            LoadSettings_(ConfigurationManager.AppSettings);

            return this;
        }

        static NarvaloWebSettings Initialize_()
        {
            return NarvaloWebSettings.FromConfiguration();
        }

        void LoadSettings_(NameValueCollection appSettings)
        {
            var keys = appSettings.AllKeys
                .Where(_ => _.StartsWith(SettingPrefix_, StringComparison.OrdinalIgnoreCase));

            var settings = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

            foreach (var key in keys) {
                settings[key] = appSettings[key];
            }

            Initialize_(settings);
        }

        void Initialize_(NameValueCollection source)
        {
            _enableWhiteSpaceBusting
                = source.MayParseValue("narvalo:enableWhiteSpaceBusting", _ => MayParse.ToBoolean(_, BooleanStyles.Literal))
                    .ValueOrElse(DefaultEnableWhiteSpaceBusting_);
        }
    }
}
