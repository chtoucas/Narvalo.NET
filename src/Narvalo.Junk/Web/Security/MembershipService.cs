namespace Narvalo.Web.Security
{
    using System.Web.Security;

    public class MembershipService : IMembershipService
    {
        readonly MembershipProvider _provider;

        public MembershipService()
            : this(null)
        {
            ;
        }

        public MembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinRequiredPasswordLength
        {
            get { return _provider.MinRequiredPasswordLength; }
        }

        public bool ValidateUser(string userName, string password)
        {
            Require.NotNullOrEmpty(userName, "userName");
            Require.NotNullOrEmpty(password, "password");

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            Require.NotNullOrEmpty(userName, "userName");
            Require.NotNullOrEmpty(password, "password");
            Require.NotNullOrEmpty(email, "email");

            MembershipCreateStatus status;

            _provider.CreateUser(userName, password, email,
                null /* passwordQuestion */, null /* passwordAnswer */, true /* isApproved */,
                null /* providerUserKey */, out status);

            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            Require.NotNullOrEmpty(userName, "userName");
            Require.NotNullOrEmpty(oldPassword, "oldPassword");
            Require.NotNullOrEmpty(newPassword, "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);

                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (MembershipPasswordException) {
                return false;
            }
        }
    }
}
