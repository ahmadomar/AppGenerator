namespace AppsGenerator.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AppsGeneratorDb : DbContext
    {
        public AppsGeneratorDb()
            : base("name=AppsGeneratorEntities")
        {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Theme> Themes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasMany(e => e.Applications)
                .WithRequired(e => e.Member)
                .HasForeignKey(e => e.member_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Theme>()
                .HasMany(e => e.Applications)
                .WithOptional(e => e.Theme)
                .HasForeignKey(e => e.theme_id);
        }
    }
}
