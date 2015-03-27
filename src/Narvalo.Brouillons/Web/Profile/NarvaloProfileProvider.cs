// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Globalization;
    using System.Web.Profile;

    public class NarvaloProfileProvider : AuthenticatedOnlyProfileProvider
    {
        static Type ProfileType_ = typeof(NarvaloProfile);

        public override string ApplicationName { get; set; }

        protected NarvaloProfileRepository Repository { get; set; }

        /// Initialize values from web.config.
        public override void Initialize(string name, NameValueCollection config)
        {
            Require.NotNull(config, "config");

            // Provider infos.
            if (String.IsNullOrEmpty(name)) {
                name = "NarvaloProfileProvider";
            }

            if (String.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "Narvalo Profile Provider");
            }

            base.Initialize(name, config);

            // Application Name.
            var applicationName = config["applicationName"].Trim();

            if (String.IsNullOrEmpty(applicationName)) {
                throw new ProviderException("You must provide an applicationName.");
            }

            ApplicationName = applicationName;

            var connectionString = config["connectionString"];

            if (String.IsNullOrEmpty(connectionString)) {
                throw new ProviderException("You must provide a connectionString.");
            }

            Repository = new NarvaloProfileRepository(connectionString);
        }

        public void DeleteProfile(string userName)
        {
            Repository.Delete(ApplicationName, userName);
        }

        public override int DeleteProfiles(string[] usernames)
        {
            Require.NotNull(usernames, "usernames");

            if (usernames.Length == 0) {
                return 0;
            }

            return Repository.Delete(ApplicationName, usernames);
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            Require.NotNull(profiles, "profiles");

            var userNames = new List<string>();

            foreach (ProfileInfo profile in profiles) {
                userNames.Add(profile.UserName);
            }

            return DeleteProfiles(userNames.ToArray());
        }

        public override ProfileInfoCollection FindProfilesByUserName(
            ProfileAuthenticationOption authenticationOption, string usernameToMatch,
            int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex < 0) {
                throw new ArgumentOutOfRangeException("pageIndex");
            }

            if (pageSize < 1) {
                throw new ArgumentOutOfRangeException("pageSize");
            }

            if (authenticationOption == ProfileAuthenticationOption.Anonymous) {
                totalRecords = 0;

                return new ProfileInfoCollection();
            }

            var profiles = Repository.Search(ApplicationName, usernameToMatch, pageIndex, pageSize, out totalRecords);

            return GetProfileInfos_(profiles);
        }

        public override ProfileInfoCollection GetAllProfiles(
            ProfileAuthenticationOption authenticationOption,
            int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex < 0) {
                throw new ArgumentOutOfRangeException("pageIndex");
            }

            if (pageSize < 1) {
                throw new ArgumentOutOfRangeException("pageSize");
            }

            if (authenticationOption == ProfileAuthenticationOption.Anonymous) {
                totalRecords = 0;

                return new ProfileInfoCollection();
            }

            var profiles = Repository.FindProfiles(ApplicationName, pageIndex, pageSize, out totalRecords);

            return GetProfileInfos_(profiles);
        }

        public override SettingsPropertyValueCollection GetPropertyValues(
            SettingsContext context, SettingsPropertyCollection collection)
        {
            Require.NotNull(context, "context");
            Require.NotNull(collection, "collection");

            var spvc = new SettingsPropertyValueCollection();

            if (collection.Count == 0) {
                return spvc;
            }

            string userName = Convert.ToString(context["UserName"], CultureInfo.InvariantCulture);

            if (String.IsNullOrEmpty(userName)) {
                return spvc;
            }

            var profile = Repository.FindByUserName(ApplicationName, userName);

            spvc = GetSettingsPropertyValues_(profile, collection);

            if (profile != null) {
                Repository.UpdateActivity(userName, true /* activityOnly */);
            }

            return spvc;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            Require.NotNull(context, "context");
            Require.NotNull(collection, "collection");

            if (!(bool)context["IsAuthenticated"]) {
                throw new NotSupportedException("This provider doesn't support anonymous profiles.");
            }

            string userName = Convert.ToString(context["UserName"], CultureInfo.InvariantCulture);

            if (String.IsNullOrEmpty(userName) | collection.Count == 0 | !IsDirty_(collection)) {
                return;
            }

            var profile = GetProfile_(userName, collection);

            Repository.SaveOrUpdate(ApplicationName, userName, profile);
            Repository.UpdateActivity(userName, false /* activityOnly */);
        }

        static ProfileInfoCollection GetProfileInfos_(IList<NarvaloProfile> profiles)
        {
            var pic = new ProfileInfoCollection();

            foreach (var profile in profiles) {
                pic.Add(new ProfileInfo(profile.UserName, false /* isAnonymous */, DateTime.Now, profile.LastUpdatedDate, 0));
            }

            return pic;
        }

        static SettingsPropertyValueCollection GetSettingsPropertyValues_(
           NarvaloProfile profile, SettingsPropertyCollection spc)
        {

            var spvc = new SettingsPropertyValueCollection();

            foreach (SettingsProperty p in spc) {
                var value = new SettingsPropertyValue(p);

                if (profile == null) {
                    value.PropertyValue = Convert.ChangeType(value.Property.DefaultValue,
                        value.Property.PropertyType, CultureInfo.InvariantCulture);
                }
                else {
                    value.PropertyValue = ProfileType_.GetProperty(p.Name).GetValue(profile, null);
                }

                value.IsDirty = false;
                value.Deserialized = true;

                spvc.Add(value);
            }

            return spvc;
        }

        static bool IsDirty_(SettingsPropertyValueCollection spvc)
        {
            foreach (SettingsPropertyValue p in spvc) {
                if (p.IsDirty) {
                    return true;
                }
            }

            return false;
        }

        NarvaloProfile GetProfile_(string userName, SettingsPropertyValueCollection spvc)
        {
            var profile = new NarvaloProfile();

            foreach (SettingsPropertyValue p in spvc) {
                ProfileType_.GetProperty(p.Name).SetValue(profile, p.PropertyValue, null);
            }

            return profile;
        }
    }
}
