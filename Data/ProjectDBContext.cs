using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectOnBoarding.Models;


namespace ProjectOnBoarding.Data
{
    public class ProjectDBContext : IdentityDbContext<ProjectDBUser>
    {
        public DbSet<Project> Projects { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Division> Divisions { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public ProjectDBContext() { }

        public ProjectDBContext(DbContextOptions<ProjectDBContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.HasDefaultSchema("dbo");
           
            builder.Entity<ProjectDBUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            //    builder.Entity<Company>()
            //.HasMany(e => e.Divisions)
            //.WithOne(e => e.Company)
            //.HasForeignKey(e => e.CompanyId)
            //.HasPrincipalKey(e => e.Id);
            //    protected override void OnModelCreating(ModelBuilder modelBuilder)
            //{

            

           //builder.Entity<Project>()
           //    .Property(p => p.Code)
           //    //.HasDefaultValueSql(" 'AUG' + '-' + DateTime.Now.Year + '-' + [Id]");
           //     //.HasComputedColumnSql($"{'AUG'}-{DateTime.Now.Year}-[Id]", stored: true);
           //     .hascom("[LastName] + ', ' + [FirstName]");

            ////Primary key for Project model
            //builder.Entity<ProjectModel>()
            //.HasKey(p => new { p.Id });

            //// Auto increment for Project model
            //builder.Entity<ProjectModel>()
            //    .Property(p => p.Id)
            //    .ValueGeneratedOnAdd();

            ////Primary key for Company model
            //builder.Entity<CompanyModel>()
            //.HasKey(c => new { c.Id });

            //// Auto increment for Company model
            //builder.Entity<CompanyModel>()
            //    .Property(c => c.Id)
            //    .ValueGeneratedOnAdd();

            ////Primary key for Division model
            //builder.Entity<DivisionModel>()
            //.HasKey(d => new { d.Id, d.CompanyId });

            //// Auto increment for Division model
            //builder.Entity<DivisionModel>()
            //    .Property(d => d.Id)
            //    .ValueGeneratedOnAdd();

        }
    }
}

