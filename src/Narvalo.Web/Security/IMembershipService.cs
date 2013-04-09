namespace Narvalo.Web.Security
{
    using System.Web.Security;

    public interface IMembershipService
    {
        int MinRequiredPasswordLength { get; }

        bool ValidateUser(string userName, string password);

        MembershipCreateStatus CreateUser(string userName, string password, string email);

        //MembershipCreateStatus CreateUser(string userName, string password, string email, out MembershipUser user);

        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }
}
