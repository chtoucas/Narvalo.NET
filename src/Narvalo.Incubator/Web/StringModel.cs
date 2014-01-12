namespace Narvalo.Web
{
    using System.ComponentModel.DataAnnotations;

    public class StringModel
    {
        [Required]
        public string Value { get; set; }
    }
}
