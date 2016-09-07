namespace WebParser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TagA")]
    public partial class TagA
    {
        public int Id { get; set; }

        public int UrlId { get; set; }

        [Required]
        [StringLength(450)]
        public string Href { get; set; }

        public virtual Urls Urls { get; set; }
    }
}
