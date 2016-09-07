namespace WebParser.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ParsingResultsEntities : DbContext
    {
        public ParsingResultsEntities()
            : base("name=ParsingResultsEntities")
        {
        }

        public virtual DbSet<TagA> TagA { get; set; }
        public virtual DbSet<TagH1> TagH1 { get; set; }
        public virtual DbSet<TagImg> TagImg { get; set; }
        public virtual DbSet<Urls> Urls { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Urls>()
                .HasMany(e => e.TagA)
                .WithRequired(e => e.Urls)
                .HasForeignKey(e => e.UrlId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Urls>()
                .HasMany(e => e.TagH1)
                .WithRequired(e => e.Urls)
                .HasForeignKey(e => e.UrlId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Urls>()
                .HasMany(e => e.TagImg)
                .WithRequired(e => e.Urls)
                .HasForeignKey(e => e.UrlId)
                .WillCascadeOnDelete(false);
        }
    }
}
