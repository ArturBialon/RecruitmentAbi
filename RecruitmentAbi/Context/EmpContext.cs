using Microsoft.EntityFrameworkCore;
using RecruitmentAbi.Models;

namespace RecruitmentAbi.Context
{
    public partial class EmpContext : DbContext
    {
        public EmpContext()
        {
        }

        public EmpContext(DbContextOptions<EmpContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Emp> Employees { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<VacationPackage> VacationPackages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Emp>()
                .HasOne(e => e.Team)
                .WithMany(t => t.Emp)
                .HasForeignKey(e => e.TeamId);

            modelBuilder.Entity<Emp>()
                .HasOne(e => e.VacationPackage)
                .WithMany(v => v.Emps)
                .HasForeignKey(e => e.VacationPackageId);

            modelBuilder.Entity<Vacation>()
                .HasOne(v => v.Emp)
                .WithMany(e => e.Vacations)
                .HasForeignKey(v => v.EmpId);
        }
    }
}
