﻿// <auto-generated />
using System;
using DateflixMVC.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DateflixMVC.Migrations
{
    [DbContext(typeof(WebApiDbContext))]
    partial class WebApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DateflixMVC.Models.Bans", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email");

                    b.HasKey("Id");

                    b.ToTable("Bans");
                });

            modelBuilder.Entity("DateflixMVC.Models.Blocks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BlockedUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Blocks");
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.ActiveLogins", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("JwtToken");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("ActiveLogins");
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.DirectMessages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Message");

                    b.Property<int>("ReceiverId");

                    b.Property<int>("SenderId");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("DirrectMessages");
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.Inquiries", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Message");

                    b.Property<string>("Type");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Inquiries");
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.Likes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("LikedId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.RolerUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("RolerUser");
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("City");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<int>("Gender");

                    b.Property<string>("LastName");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("ProfilePictures");

                    b.Property<DateTime?>("UpdatedDate");

                    b.Property<int?>("UserPreferenceId");

                    b.HasKey("Id");

                    b.HasIndex("UserPreferenceId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.UserPreference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Gender");

                    b.Property<int>("MaximumAge");

                    b.Property<int>("MinimumAge");

                    b.HasKey("Id");

                    b.ToTable("UserPreference");
                });

            modelBuilder.Entity("DateflixMVC.Models.Blocks", b =>
                {
                    b.HasOne("DateflixMVC.Models.Profile.User")
                        .WithMany("Blocks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.DirectMessages", b =>
                {
                    b.HasOne("DateflixMVC.Models.Profile.User")
                        .WithMany("DirectMessages")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.Inquiries", b =>
                {
                    b.HasOne("DateflixMVC.Models.Profile.User")
                        .WithMany("Inquiries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.Likes", b =>
                {
                    b.HasOne("DateflixMVC.Models.Profile.User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.RolerUser", b =>
                {
                    b.HasOne("DateflixMVC.Models.Profile.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DateflixMVC.Models.Profile.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DateflixMVC.Models.Profile.User", b =>
                {
                    b.HasOne("DateflixMVC.Models.Profile.UserPreference", "UserPreference")
                        .WithMany()
                        .HasForeignKey("UserPreferenceId");
                });
#pragma warning restore 612, 618
        }
    }
}
