﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechSupportApp.Infrastructure.Persistence;

namespace TechSupportApp.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20201224123455_User")]
    partial class User
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("TechSupportApp.Domain.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Issue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IssuerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TicketStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IssuerId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("TechSupportApp.Domain.Models.TrackEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketEntries");
                });

            modelBuilder.Entity("TechSupportApp.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TechSupportApp.Domain.Models.Ticket", b =>
                {
                    b.HasOne("TechSupportApp.Domain.Models.User", "Issuer")
                        .WithMany("Tickets")
                        .HasForeignKey("IssuerId");

                    b.Navigation("Issuer");
                });

            modelBuilder.Entity("TechSupportApp.Domain.Models.TrackEntry", b =>
                {
                    b.HasOne("TechSupportApp.Domain.Models.User", "Author")
                        .WithMany("TrackEntries")
                        .HasForeignKey("AuthorId");

                    b.HasOne("TechSupportApp.Domain.Models.Ticket", null)
                        .WithMany("Track")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("TechSupportApp.Domain.Models.Ticket", b =>
                {
                    b.Navigation("Track");
                });

            modelBuilder.Entity("TechSupportApp.Domain.Models.User", b =>
                {
                    b.Navigation("Tickets");

                    b.Navigation("TrackEntries");
                });
#pragma warning restore 612, 618
        }
    }
}