namespace Narvalo.Web.Profile
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Profile;

    public class NarvaloProfile : ProfileBase
    {
        [SettingsAllowAnonymous(false)]
        [ProfileProvider("NarvaloProfileProvider")]
        public string City
        {
            get { return base["City"] as string; }
            set { base["City"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        [ProfileProvider("NarvaloProfileProvider")]
        public string Country
        {
            get { return base["Country"] as string; }
            set { base["Country"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        [Required(ErrorMessage = "the email is required")]
        [ProfileProvider("NarvaloProfileProvider")]
        public string Email
        {
            get { return base["Email"] as string; }
            set { base["Email"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        [Required(ErrorMessage = "the firstname is required")]
        [ProfileProvider("NarvaloProfileProvider")]
        public string Firstname
        {
            get { return base["Firstname"] as string; }
            set { base["Firstname"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        [ProfileProvider("NarvaloProfileProvider")]
        public string MobileNumber
        {
            get { return (string)base["MobileNumber"]; }
            set { base["MobileNumber"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        [Required(ErrorMessage = "the name is required")]
        [ProfileProvider("NarvaloProfileProvider")]
        public string Name
        {
            get { return base["Name"] as string; }
            set { base["Name"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        [ProfileProvider("NarvaloProfileProvider")]
        public string PhoneNumber
        {
            get { return (string)base["PhoneNumber"]; }
            set { base["PhoneNumber"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        [ProfileProvider("NarvaloProfileProvider")]
        public string Street
        {
            get { return base["Street"] as string; }
            set { base["Street"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        [ProfileProvider("NarvaloProfileProvider")]
        public string ZipCode
        {
            get { return base["ZipCode"] as string; }
            set { base["ZipCode"] = value; }
        }

        //public static NarvaloProfile GetProfile()
        //{
        //    return HttpContext.Current.Profile as NarvaloProfile;
        //}

        //public static NarvaloProfile GetProfile(string userName)
        //{
        //    return Create(userName) as NarvaloProfile;
        //}
    }
}
