// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Profile
{
    using System;
    using System.Collections.Generic;

    public class NarvaloProfileRepository
    {
        readonly string _connectionString;

        public NarvaloProfileRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Count(string appName)
        {
            throw new NotImplementedException();
        }

        public void Delete(string appName, string userName)
        {
            throw new NotImplementedException();
        }

        public int Delete(string appName, string[] userNames)
        {
            throw new NotImplementedException();
        }

        public NarvaloProfile FindByUserName(string appName, string userName)
        {
            throw new NotImplementedException();
        }

        public IList<NarvaloProfile> FindProfiles(string appName, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public void SaveOrUpdate(string appName, string userName, NarvaloProfile profile)
        {
            throw new NotImplementedException();
        }

        public IList<NarvaloProfile> Search(string appName, string userName, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public void UpdateActivity(string userName, bool activityOnly)
        {
            throw new NotImplementedException();
        }
    }
}
