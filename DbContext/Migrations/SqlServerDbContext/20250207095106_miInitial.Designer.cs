﻿// <auto-generated />
using System;
using DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    [DbContext(typeof(MainDbContext.SqlServerDbContext))]
<<<<<<<< HEAD:DbContext/Migrations/SqlServerDbContext/20250207095023_miInitial.Designer.cs
    [Migration("20250207095023_miInitial")]
========
    [Migration("20250207095106_miInitial")]
>>>>>>>> martin-attraction-comments-fix:DbContext/Migrations/SqlServerDbContext/20250207095106_miInitial.Designer.cs
    partial class miInitial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DbModels.AddressDbM", b =>
                {
                    b.Property<Guid>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses", "supusr");
                });

            modelBuilder.Entity("DbModels.AttractionDbM", b =>
                {
                    b.Property<Guid>("AttractionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AddressDbMAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AttractionTitle")
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid>("CategoryDbMCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.HasKey("AttractionId");

                    b.HasIndex("AddressDbMAddressId");

                    b.HasIndex("CategoryDbMCategoryId");

                    b.ToTable("Attractions", "supusr");
                });

            modelBuilder.Entity("DbModels.CategoryDbM", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Catkind")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories", "supusr");
                });

            modelBuilder.Entity("DbModels.CommentDbM", b =>
                {
                    b.Property<Guid>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttractionDbMAttractionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.Property<int>("Sentiment")
                        .HasColumnType("int");

                    b.Property<string>("strSentiment")
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("CommentId");

                    b.HasIndex("AttractionDbMAttractionId");

                    b.ToTable("Comments", "supusr");
                });

            modelBuilder.Entity("DbModels.RoleDbM", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Rolekind")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Roles")
                        .HasColumnType("int");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DbModels.UserDbM", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("UserId");

                    b.ToTable("Users", "dbo");
                });

            modelBuilder.Entity("Models.DTO.GstUsrInfoCommentsDto", b =>
                {
                    b.ToTable((string)null);

                    b.ToView("vwInfoComments", "gstusr");
                });

            modelBuilder.Entity("Models.DTO.GstUsrInfoDbDto", b =>
                {
                    b.Property<int>("NrSeededAddresses")
                        .HasColumnType("int");

                    b.Property<int>("NrSeededAttractions")
                        .HasColumnType("int");

                    b.Property<int>("NrSeededCategories")
                        .HasColumnType("int");

                    b.Property<int>("NrSeededComments")
                        .HasColumnType("int");

                    b.Property<int>("NrUnseededAddresses")
                        .HasColumnType("int");

                    b.Property<int>("NrUnseededAttractions")
                        .HasColumnType("int");

                    b.Property<int>("NrUnseededCategories")
                        .HasColumnType("int");

                    b.Property<int>("NrUnseededComments")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("vwInfoDb", "gstusr");
                });

            modelBuilder.Entity("DbModels.AttractionDbM", b =>
                {
                    b.HasOne("DbModels.AddressDbM", "AddressDbM")
                        .WithMany("AttractionDbM")
                        .HasForeignKey("AddressDbMAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DbModels.CategoryDbM", "CategoryDbM")
                        .WithMany("AttractionsDbM")
                        .HasForeignKey("CategoryDbMCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AddressDbM");

                    b.Navigation("CategoryDbM");
                });

            modelBuilder.Entity("DbModels.CommentDbM", b =>
                {
                    b.HasOne("DbModels.AttractionDbM", "AttractionDbM")
                        .WithMany("CommentsDbM")
                        .HasForeignKey("AttractionDbMAttractionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AttractionDbM");
                });

            modelBuilder.Entity("DbModels.AddressDbM", b =>
                {
                    b.Navigation("AttractionDbM");
                });

            modelBuilder.Entity("DbModels.AttractionDbM", b =>
                {
                    b.Navigation("CommentsDbM");
                });

            modelBuilder.Entity("DbModels.CategoryDbM", b =>
                {
                    b.Navigation("AttractionsDbM");
                });
#pragma warning restore 612, 618
        }
    }
}
