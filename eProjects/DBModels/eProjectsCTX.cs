using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eProjects.DBModels
{
    public partial class eProjectsCTX : DbContext
    {
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Eiresource> Eiresource { get; set; }
        public virtual DbSet<FundingType> FundingType { get; set; }
        public virtual DbSet<ImpactedDepartment> ImpactedDepartment { get; set; }
        public virtual DbSet<Leaders> Leaders { get; set; }
        public virtual DbSet<LeadingDepartment> LeadingDepartment { get; set; }
        public virtual DbSet<Masterplan> Masterplan { get; set; }
        public virtual DbSet<Pcisresource> Pcisresource { get; set; }
        public virtual DbSet<ProjectType> ProjectType { get; set; }
        public virtual DbSet<Ptresource> Ptresource { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Top3> Top3 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Data Source=TESTINTERN-PC\SQLEXPRESS;Initial Catalog=eProjects;Integrated Security=False;User ID=sa;Password=Parola00;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Fullname).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Eiresource>(entity =>
            {
                entity.ToTable("EIResource");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FundingType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ImpactedDepartment>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Leaders>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LeadingDepartment>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Masterplan>(entity =>
            {
                entity.Property(e => e.ActualEndDate).HasColumnType("datetime");

                entity.Property(e => e.ActualEndFiscalYear).HasMaxLength(10);

                entity.Property(e => e.ActualSpendingTargetEtc).HasColumnName("ActualSpendingTargetETC");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Cm).HasColumnName("CM");

                entity.Property(e => e.ConceptualActualEnd).HasColumnType("datetime");

                entity.Property(e => e.ConceptualEnd).HasColumnType("datetime");

                entity.Property(e => e.DefinitionActualEnd).HasColumnType("datetime");

                entity.Property(e => e.DefinitionEnd).HasColumnType("datetime");

                entity.Property(e => e.DesignConstructActualEnd).HasColumnType("datetime");

                entity.Property(e => e.DesignConstructEnd).HasColumnType("datetime");

                entity.Property(e => e.DesignIqpqprotocol).HasColumnName("DesignIQPQProtocol");

                entity.Property(e => e.Eiresource)
                    .HasColumnName("EIResource")
                    .HasMaxLength(50);

                entity.Property(e => e.Etc).HasColumnName("ETC");

                entity.Property(e => e.FesabilityActualEnd).HasColumnType("datetime");

                entity.Property(e => e.FesabilityEnd).HasColumnType("datetime");

                entity.Property(e => e.FiscalYearStart)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.FundingType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ImpactedDepartment)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LeadingDepartment)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ora)
                    .HasColumnName("ORA")
                    .HasColumnType("decimal(18, 1)");

                entity.Property(e => e.PcisResource).HasMaxLength(50);

                entity.Property(e => e.PlantAe).HasColumnName("PlantAE");

                entity.Property(e => e.PredictedEndDate).HasColumnType("datetime");

                entity.Property(e => e.PredictedEndFiscalYear)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ProjectLeader).IsRequired();

                entity.Property(e => e.ProjectName).IsRequired();

                entity.Property(e => e.ProjectType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ptresource)
                    .HasColumnName("PTResource")
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.StartupLeader).IsRequired();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Pcisresource>(entity =>
            {
                entity.ToTable("PCISResource");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ProjectType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Ptresource>(entity =>
            {
                entity.ToTable("PTResource");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Top3>(entity =>
            {
                entity.Property(e => e.ProjectLeader).IsRequired();
            });
        }
    }
}
