//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;

//namespace Evolution.Security.Domain.Models.SqlDatabaseContext
//{
//    public partial class SecuritySqlDbContext : DbContext
//    {
//        public SecuritySqlDbContext()
//        {
//        }

//        public SecuritySqlDbContext(DbContextOptions<SecuritySqlDbContext> options)
//            : base(options)
//        {
//        }

//        public virtual DbSet<Activity> Activity { get; set; }
//        public virtual DbSet<Application> Application { get; set; }
//        public virtual DbSet<ApplicationMenu> ApplicationMenu { get; set; }
//        public virtual DbSet<Module> Module { get; set; }
//        public virtual DbSet<ModuleActivity> ModuleActivity { get; set; }
//        public virtual DbSet<Role> Role { get; set; }
//        public virtual DbSet<RoleActivity> RoleActivity { get; set; }
//        public virtual DbSet<User> User { get; set; }
//        public virtual DbSet<UserRole> UserRole { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=192.168.50.172;Database=Evolution2_Test3;user=sa;password=sa123;");
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

//            modelBuilder.Entity<Activity>(entity =>
//            {
//                entity.ToTable("Activity", "security");

//                entity.HasIndex(e => new { e.ApplicationId, e.Code, e.Name })
//                    .HasName("IX_Activity_AppId_Code_Name")
//                    .IsUnique();

//                entity.Property(e => e.Code)
//                    .IsRequired()
//                    .HasMaxLength(15);

//                entity.Property(e => e.Description).HasMaxLength(500);

//                entity.Property(e => e.LastModification).HasColumnType("datetime");

//                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

//                entity.Property(e => e.Name)
//                    .IsRequired()
//                    .HasMaxLength(50);

//                entity.HasOne(d => d.Application)
//                    .WithMany(p => p.Activity)
//                    .HasForeignKey(d => d.ApplicationId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_Activity_Activity_ApplicationId");
//            });

//            modelBuilder.Entity<Application>(entity =>
//            {
//                entity.ToTable("Application", "security");

//                entity.HasIndex(e => e.Name)
//                    .IsUnique();

//                entity.Property(e => e.Description).HasMaxLength(500);

//                entity.Property(e => e.LastModification).HasColumnType("datetime");

//                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

//                entity.Property(e => e.Name)
//                    .IsRequired()
//                    .HasMaxLength(100);
//            });

//            modelBuilder.Entity<ApplicationMenu>(entity =>
//            {
//                entity.ToTable("ApplicationMenu", "security");

//                entity.HasIndex(e => new { e.ApplicationId, e.ModuleId, e.MenuName })
//                    .HasName("IX_ApplicationMenu_Application_Module_MenuName")
//                    .IsUnique();

//                entity.Property(e => e.ActivitiesCode).HasMaxLength(500);

//                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

//                entity.Property(e => e.MenuName)
//                    .IsRequired()
//                    .HasMaxLength(100);

//                entity.HasOne(d => d.Application)
//                    .WithMany(p => p.ApplicationMenu)
//                    .HasForeignKey(d => d.ApplicationId)
//                    .OnDelete(DeleteBehavior.ClientSetNull);

//                entity.HasOne(d => d.Module)
//                    .WithMany(p => p.ApplicationMenu)
//                    .HasForeignKey(d => d.ModuleId)
//                    .OnDelete(DeleteBehavior.ClientSetNull);
//            });

//            modelBuilder.Entity<Module>(entity =>
//            {
//                entity.ToTable("Module", "security");

//                entity.HasIndex(e => new { e.ApplicationId, e.Name })
//                    .IsUnique();

//                entity.Property(e => e.Description).HasMaxLength(100);

//                entity.Property(e => e.LastModification).HasColumnType("datetime");

//                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

//                entity.Property(e => e.Name)
//                    .IsRequired()
//                    .HasMaxLength(50);

//                entity.HasOne(d => d.Application)
//                    .WithMany(p => p.Module)
//                    .HasForeignKey(d => d.ApplicationId)
//                    .OnDelete(DeleteBehavior.ClientSetNull);
//            });

//            modelBuilder.Entity<ModuleActivity>(entity =>
//            {
//                entity.ToTable("ModuleActivity", "security");

//                entity.HasIndex(e => new { e.ActivityId, e.MouduleId })
//                    .HasName("IX_Module_Activity_ActivityId_ModuleId")
//                    .IsUnique();

//                entity.Property(e => e.LastModification).HasColumnType("datetime");

//                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

//                entity.HasOne(d => d.Activity)
//                    .WithMany(p => p.ModuleActivity)
//                    .HasForeignKey(d => d.ActivityId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_ModuleActivity_ActivityId");

//                entity.HasOne(d => d.Moudule)
//                    .WithMany(p => p.ModuleActivity)
//                    .HasForeignKey(d => d.MouduleId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_ModuleActivity_Module_ModuleId");
//            });

//            modelBuilder.Entity<Role>(entity =>
//            {
//                entity.ToTable("Role", "security");

//                entity.HasIndex(e => new { e.ApplicationId, e.Name })
//                    .HasName("IX_Role_Name_ApplicationId")
//                    .IsUnique();

//                entity.Property(e => e.Description).HasMaxLength(200);

//                entity.Property(e => e.LastModification).HasColumnType("datetime");

//                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

//                entity.Property(e => e.Name)
//                    .IsRequired()
//                    .HasMaxLength(50);

//                entity.HasOne(d => d.Application)
//                    .WithMany(p => p.Role)
//                    .HasForeignKey(d => d.ApplicationId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_Role_ApplicationId");
//            });

//            modelBuilder.Entity<RoleActivity>(entity =>
//            {
//                entity.ToTable("RoleActivity", "security");

//                entity.HasIndex(e => new { e.RoleId, e.ActivityId, e.ModuleId })
//                    .HasName("IX_RoleActivity_RoleId_ActivityId")
//                    .IsUnique();

//                entity.Property(e => e.LastModification).HasColumnType("datetime");

//                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

//                entity.HasOne(d => d.Activity)
//                    .WithMany(p => p.RoleActivity)
//                    .HasForeignKey(d => d.ActivityId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_RoleActivity_RoleActivity_ActivityId");

//                entity.HasOne(d => d.Module)
//                    .WithMany(p => p.RoleActivity)
//                    .HasForeignKey(d => d.ModuleId)
//                    .OnDelete(DeleteBehavior.ClientSetNull);

//                entity.HasOne(d => d.Role)
//                    .WithMany(p => p.RoleActivity)
//                    .HasForeignKey(d => d.RoleId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_RoleActivity_RoleId");
//            });

//            modelBuilder.Entity<User>(entity =>
//            {
//                entity.ToTable("User", "security");

//                entity.HasIndex(e => new { e.SamaccountName, e.ApplicationId })
//                    .IsUnique();

//                entity.Property(e => e.Culture).HasMaxLength(10);

//                entity.Property(e => e.Email)
//                    .IsRequired()
//                    .HasMaxLength(50);

//                entity.Property(e => e.IsActive)
//                    .IsRequired()
//                    .HasDefaultValueSql("((1))");

//                entity.Property(e => e.LastModification).HasColumnType("datetime");

//                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

//                entity.Property(e => e.Name)
//                    .IsRequired()
//                    .HasMaxLength(50);

//                entity.Property(e => e.SamaccountName)
//                    .IsRequired()
//                    .HasColumnName("SAMAccountName")
//                    .HasMaxLength(50);

//                entity.HasOne(d => d.Application)
//                    .WithMany(p => p.User)
//                    .HasForeignKey(d => d.ApplicationId)
//                    .OnDelete(DeleteBehavior.ClientSetNull);
//            });

//            modelBuilder.Entity<UserRole>(entity =>
//            {
//                entity.ToTable("UserRole", "security");

//                entity.HasIndex(e => new { e.RoleId, e.UserId, e.CompanyId })
//                    .HasName("IX_UserRole_UID_RID_CID")
//                    .IsUnique();

//                entity.Property(e => e.LastModification).HasColumnType("datetime");

//                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

//                entity.HasOne(d => d.Role)
//                    .WithMany(p => p.UserRole)
//                    .HasForeignKey(d => d.RoleId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_UserRole_RoleId");

//                entity.HasOne(d => d.User)
//                    .WithMany(p => p.UserRole)
//                    .HasForeignKey(d => d.UserId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_UserRole_UserId");
//            });
//        }
//    }
//}
