// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Profile
{
    using System;
    using System.Web.Profile;

    public abstract class AuthenticatedOnlyProfileProvider : ProfileProvider
    {
        protected AuthenticatedOnlyProfileProvider() : base() { }

        #region Unsupported methods: no support for anonymous profiles.

        public override int DeleteInactiveProfiles(
            ProfileAuthenticationOption authenticationOption,
            DateTime userInactiveSinceDate)
        {
            throw new NotSupportedException("This provider does not support anonymous profiles");
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(
            ProfileAuthenticationOption authenticationOption,
            string usernameToMatch, DateTime userInactiveSinceDate,
            int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException("This provider does not support anonymous profiles");
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(
            ProfileAuthenticationOption authenticationOption,
            DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException("This provider does not support anonymous profiles");
        }

        public override int GetNumberOfInactiveProfiles(
            ProfileAuthenticationOption authenticationOption,
            DateTime userInactiveSinceDate)
        {
            throw new NotSupportedException("This provider does not support anonymous profiles");
        }

        #endregion
    }
}
