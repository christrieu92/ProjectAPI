﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using yortrip.DbContexts;

namespace yortrip.Migrations
{
    [DbContext(typeof(CalenderContext))]
    [Migration("20201121175608_Database_and_Store_Procedure_Changes")]
    partial class Database_and_Store_Procedure_Changes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("yortrip.Entities.AvailabilityDate", b =>
                {
                    b.Property<DateTime>("AvailableDate")
                        .HasColumnType("datetime2");

                    b.HasKey("AvailableDate");

                    b.ToTable("AvailabilityDate");
                });

            modelBuilder.Entity("yortrip.Entities.AvailabilityDateRange", b =>
                {
                    b.Property<DateTime>("StartRange")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndRange")
                        .HasColumnType("datetime2");

                    b.HasKey("StartRange");

                    b.ToTable("AvailabilityDateRange");
                });

            modelBuilder.Entity("yortrip.Entities.CalendarModel", b =>
                {
                    b.Property<Guid>("CalendarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndMonth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartMonth")
                        .HasColumnType("datetime2");

                    b.HasKey("CalendarId");

                    b.ToTable("Calendars");
                });

            modelBuilder.Entity("yortrip.Entities.Notification", b =>
                {
                    b.Property<Guid>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CalendarId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Viewed")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotificationId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("yortrip.Entities.Unavailability", b =>
                {
                    b.Property<Guid>("UnavailabilityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CalendarId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UnavailabilityDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UnavailabilityId");

                    b.ToTable("Unavailabilities");
                });

            modelBuilder.Entity("yortrip.Entities.UnavailabilityDate", b =>
                {
                    b.Property<DateTime>("UnavailableDate")
                        .HasColumnType("datetime2");

                    b.HasKey("UnavailableDate");

                    b.ToTable("UnavailabilityDate");
                });

            modelBuilder.Entity("yortrip.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FireBaseUID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("yortrip.Entities.UserCalendar", b =>
                {
                    b.Property<Guid>("UserCalendarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CalendarId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserCalendarId");

                    b.ToTable("UsersCalendar");
                });
#pragma warning restore 612, 618
        }
    }
}
