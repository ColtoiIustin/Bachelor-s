using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DatabaseModel
{
    public partial class DatabaseEntitiesModel : DbContext
    {
        public DatabaseEntitiesModel()
            : base("name=DatabaseEntitiesModel")
        {
        }

        public virtual DbSet<Angajati> Angajati { get; set; }
        public virtual DbSet<Plantatie> Plantatie { get; set; }
        public virtual DbSet<Proiecte> Proiecte { get; set; }
        public virtual DbSet<Stoc> Stoc { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stoc>()
                .Property(e => e.Cantitate)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Stoc>()
                .Property(e => e.Pret)
                .HasPrecision(5, 2);
        }
    }
}
