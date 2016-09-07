namespace WebParser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Urls
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Urls()
        {
            TagA = new HashSet<TagA>();
            TagH1 = new HashSet<TagH1>();
            TagImg = new HashSet<TagImg>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Url { get; set; }

        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(250)]
        public string MetaContent { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string StatusDescription { get; set; }

        [StringLength(50)]
        public string StatusCode { get; set; }

        public DateTime? DateOfParsing { get; set; }

        public DateTime? TimeToFirstBit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TagA> TagA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TagH1> TagH1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TagImg> TagImg { get; set; }
    }
}
