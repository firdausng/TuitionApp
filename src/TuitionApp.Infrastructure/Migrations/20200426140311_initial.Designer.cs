﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TuitionApp.Infrastructure.Data;

namespace TuitionApp.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200426140311_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Classroom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Classrooms");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Rate")
                        .HasColumnType("integer");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Enrollment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Grade")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TimetableId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("TimetableId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.InstructorTimetable", b =>
                {
                    b.Property<Guid>("InstructorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TimetableId")
                        .HasColumnType("uuid");

                    b.HasKey("InstructorId", "TimetableId");

                    b.HasIndex("TimetableId");

                    b.ToTable("InstructorTimetable");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.LocationInstructor", b =>
                {
                    b.Property<Guid>("LocationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("InstructorId")
                        .HasColumnType("uuid");

                    b.HasKey("LocationId", "InstructorId");

                    b.HasIndex("InstructorId");

                    b.ToTable("LocationInstructor");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("PersonRole")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Person");

                    b.HasDiscriminator<string>("PersonRole").HasValue("Person");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Timeslot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClassroomId")
                        .HasColumnType("uuid");

                    b.Property<int>("Day")
                        .HasColumnType("integer");

                    b.Property<bool>("Disabled")
                        .HasColumnType("boolean");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("interval");

                    b.Property<Guid>("TimetableId")
                        .HasColumnType("uuid");

                    b.Property<int>("WeekNumber")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomId");

                    b.HasIndex("TimetableId");

                    b.ToTable("Timeslots");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Timetable", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Timetables");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Instructor", b =>
                {
                    b.HasBaseType("TuitionApp.Core.Domain.Entities.Person");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasDiscriminator().HasValue("Instructor");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Student", b =>
                {
                    b.HasBaseType("TuitionApp.Core.Domain.Entities.Person");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Classroom", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Location", "Location")
                        .WithMany("Classrooms")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Enrollment", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Student", "Student")
                        .WithMany("Enrollments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuitionApp.Core.Domain.Entities.Timetable", "Timetable")
                        .WithMany("Enrollments")
                        .HasForeignKey("TimetableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.InstructorTimetable", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Timetable", "Timetable")
                        .WithMany("InstructorTimetables")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuitionApp.Core.Domain.Entities.Instructor", "Instructor")
                        .WithMany("InstructorTimetables")
                        .HasForeignKey("TimetableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.LocationInstructor", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Location", "Location")
                        .WithMany("LocationInstructors")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuitionApp.Core.Domain.Entities.Instructor", "Instructor")
                        .WithMany("LocationInstructors")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Timeslot", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Classroom", "Classroom")
                        .WithMany("Timeslots")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuitionApp.Core.Domain.Entities.Timetable", "Timetable")
                        .WithMany()
                        .HasForeignKey("TimetableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Timetable", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Course", "Course")
                        .WithMany("Timetables")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}