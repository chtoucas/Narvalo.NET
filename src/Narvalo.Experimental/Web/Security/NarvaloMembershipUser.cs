namespace Narvalo.Web.Security
{
    using System;
    using System.Web.Security;

    public class NarvaloMembershipUser : MembershipUser {
        public NarvaloMembershipUser(string providername,
            string username,
            object providerUserKey,
            string email,
            string passwordQuestion,
            string comment,
            bool isApproved,
            bool isLockedOut,
            DateTime creationDate,
            DateTime lastLoginDate,
            DateTime lastActivityDate,
            DateTime lastPasswordChangedDate,
            DateTime lastLockedOutDate,
            bool isSubscriber,
            string customerID)
            : base(providername, username,
                providerUserKey,
                email,
                passwordQuestion,
                comment,
                isApproved,
                isLockedOut,
                creationDate,
                lastLoginDate,
                lastActivityDate,
                lastPasswordChangedDate,
                lastLockedOutDate) {

            IsSubscriber = isSubscriber;
            CustomerID = customerID;
        }

        public bool IsSubscriber { get; set; }

        public string CustomerID { get; set; }
    }
}
