namespace Narvalo.Web.Security
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Web.Security;
    using Narvalo;
    using Narvalo.Web.Profile;

    public class NarvaloMembershipProvider : SqlMembershipProvider
    {
        protected NarvaloProfileRepository Repository { get; set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            Require.NotNull(config, "config");

            if (String.IsNullOrEmpty(name)) {
                name = "NarvaloMembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "Narvalo Membership Provider");
            }

            base.Initialize(name, config);

            //var cpa = (IContainerProviderAccessor)HttpContext.Current.ApplicationInstance;
            //var cp = cpa.ContainerProvider;
            //ProfileService = cp.RequestLifetime.Resolve<NarvaloProfileService>();

            var connectionString = config["connectionString"];

            if (String.IsNullOrEmpty(connectionString)) {
                throw new ProviderException("You must provide a connectionString.");
            }

            Repository = new NarvaloProfileRepository(connectionString);
        }

        public override MembershipUser CreateUser(
            string userName, string password,
            string email, string passwordQuestion, string passwordAnswer,
            bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            MembershipUser user = base.CreateUser(userName, password, email, passwordQuestion,
                passwordAnswer, isApproved, providerUserKey, out status);

            if (status == MembershipCreateStatus.Success) {
                Repository.SaveOrUpdate(
                    "FIXME",
                    userName,
                    new NarvaloProfile {
                        Email = user.Email,
                    }
                );
            }

            return user;
        }

        public MembershipUser CreateUser(
            string userName, string password,
            string email, string passwordQuestion, string passwordAnswer,
            string phoneNumber, string mobileNumber, string street, string zipCode,
            string city, string country,
            bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            MembershipUser user = base.CreateUser(userName, password, email, passwordQuestion,
                passwordAnswer, isApproved, providerUserKey, out status);

            if (status == MembershipCreateStatus.Success) {
                Repository.SaveOrUpdate(
                    "FIXME",
                    userName,
                    new NarvaloProfile {
                        Email = user.Email,
                        Street = street,
                        ZipCode = zipCode,
                        City = city,
                        Country = country,
                        PhoneNumber = phoneNumber,
                        MobileNumber = mobileNumber
                    }
                );
            }

            return user;
        }
    }
}
