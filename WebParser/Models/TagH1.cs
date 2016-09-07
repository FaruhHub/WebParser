namespace WebParser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TagH1
    {
        public int Id { get; set; }

        public int UrlId { get; set; }

        [Required]
        [StringLength(350)]
        public string H1Text { get; set; }

        public virtual Urls Urls { get; set; }
    }
}
