namespace WebParser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TagImg")]
    public partial class TagImg
    {
        public int Id { get; set; }

        public int UrlId { get; set; }

        [Required]
        [StringLength(250)]
        public string Src { get; set; }

        public virtual Urls Urls { get; set; }
    }
}
