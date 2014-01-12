namespace Narvalo.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ValueModel<T> where T : struct
    {
        [Required]
        public Nullable<T> Value { get; set; }
    }
}
