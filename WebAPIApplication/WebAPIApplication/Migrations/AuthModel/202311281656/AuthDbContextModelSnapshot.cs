﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAPIApplication.DbContext.Auth;

#nullable disable

namespace WebAPIApplication.Migrations.AuthModel._202311281656
{
    [DbContext(typeof(AuthDbContext))]
    partial class AuthDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebAPIApplication.Model.Auth.Permissions", b =>
                {
                    b.Property<string>("PermissionsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ControllerName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Routers")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PermissionsId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("WebAPIApplication.Model.Auth.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebAPIApplication.Model.Auth.UserPermissions", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PermissionsId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "PermissionsId");

                    b.HasIndex("PermissionsId");

                    b.ToTable("UserPermissions");
                });

            modelBuilder.Entity("WebAPIApplication.Model.Auth.UserPermissions", b =>
                {
                    b.HasOne("WebAPIApplication.Model.Auth.Permissions", "Permissions")
                        .WithMany("UserPermissionsList")
                        .HasForeignKey("PermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPIApplication.Model.Auth.User", "User")
                        .WithMany("UserPermissionsList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permissions");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebAPIApplication.Model.Auth.Permissions", b =>
                {
                    b.Navigation("UserPermissionsList");
                });

            modelBuilder.Entity("WebAPIApplication.Model.Auth.User", b =>
                {
                    b.Navigation("UserPermissionsList");
                });
#pragma warning restore 612, 618
        }
    }
}