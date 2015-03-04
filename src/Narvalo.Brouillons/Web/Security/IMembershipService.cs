// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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
