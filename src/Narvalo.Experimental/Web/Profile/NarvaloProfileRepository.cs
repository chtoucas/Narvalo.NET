namespace Narvalo.Web.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.Practices.EnterpriseLibrary.Caching;

    public class NarvaloProfileRepository
    {
        const string CacheManagerName_ = "Profiles";

        readonly ICacheManager _cacheManager;
        readonly string _connectionString;

        public NarvaloProfileRepository(string connectionString)
        {
            _connectionString = connectionString;
            _cacheManager = CacheFactory.GetCacheManager(CacheManagerName_);
        }

        public int Count(string appName)
        {
            throw new NotImplementedException();
        }

        public void Delete(string appName, string userName)
        {
            DeleteCache_(appName, userName);

            throw new NotImplementedException();
        }

        public int Delete(string appName, string[] userNames)
        {
            throw new NotImplementedException();
        }

        public NarvaloProfile FindByUserName(string appName, string userName)
        {
            NarvaloProfile profile = GetCache_(appName, userName);

            if (profile == null) {
                //profile = FindByUserName(appName, userName);

                if (profile != null) {
                    AddCache_(appName, userName, profile);
                }
            }

            return profile;
        }

        public IList<NarvaloProfile> FindProfiles(string appName, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public void SaveOrUpdate(string appName, string userName, NarvaloProfile profile)
        {
            DeleteCache_(appName, userName);

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

        #region Méthodes privées

        void AddCache_(string appName, string userName, NarvaloProfile profile)
        {
            string cacheKey = GetCacheKey_(appName, userName);

            _cacheManager.Add(cacheKey, profile);
        }

        NarvaloProfile GetCache_(string appName, string userName)
        {
            string cacheKey = GetCacheKey_(appName, userName);

            NarvaloProfile profile = null;

            if (_cacheManager.Contains(cacheKey)) {
                profile = _cacheManager.GetData(cacheKey) as NarvaloProfile;
            }

            return profile;
        }

        void DeleteCache_(string appName, string userName)
        {
            _cacheManager.Remove(GetCacheKey_(appName, userName));
        }

        string GetCacheKey_(string appName, string username)
        {
            return String.Format(CultureInfo.InvariantCulture,
                "{0}_{1}",
                appName,
                username.ToUpperInvariant());
        }

        #endregion
    }
}
