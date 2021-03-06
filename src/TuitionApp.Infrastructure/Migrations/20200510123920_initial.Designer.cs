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
    [Migration("20200510123920_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "5.0.0-preview.3.20181.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Attendance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AttendanceStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("EnrollmentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TimeslotId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("EnrollmentId");

                    b.HasIndex("TimeslotId");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.CalendarSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("AllowedTimeslotOverlap")
                        .HasColumnType("boolean");

                    b.Property<TimeSpan>("DefaultClosingTime")
                        .HasColumnType("interval");

                    b.Property<TimeSpan>("DefaultOpeningTime")
                        .HasColumnType("interval");

                    b.Property<int>("FirstDayOfWeek")
                        .HasColumnType("integer");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("CalendarSettings");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.ClassSubject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseClassId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubjectAssignmentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CourseClassId");

                    b.HasIndex("SubjectAssignmentId");

                    b.ToTable("ClassSubjects");
                });

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

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.CourseClass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("LocationId");

                    b.ToTable("CourseClasses");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.CourseSubject", b =>
                {
                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uuid");

                    b.HasKey("CourseId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("CourseSubject");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.DailySchedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClassroomId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateSchedule")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("integer");

                    b.Property<bool>("Disabled")
                        .HasColumnType("boolean");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<int>("WeekNumber")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomId");

                    b.ToTable("DailySchedules");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Enrollment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseClassId")
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

                    b.HasKey("Id");

                    b.HasIndex("CourseClassId");

                    b.HasIndex("StudentId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<TimeSpan>("ClosingTime")
                        .HasColumnType("interval");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<TimeSpan>("OpeningTime")
                        .HasColumnType("interval");

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

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.SubjectAssignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("InstructorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("InstructorId");

                    b.HasIndex("SubjectId");

                    b.ToTable("SubjectAssignments");
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Timeslot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClassSubjectId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ClassroomId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DailyScheduleId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Disabled")
                        .HasColumnType("boolean");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("interval");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ClassSubjectId");

                    b.HasIndex("ClassroomId");

                    b.HasIndex("DailyScheduleId");

                    b.ToTable("Timeslots");
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

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Attendance", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Enrollment", "Enrollment")
                        .WithMany("Attendances")
                        .HasForeignKey("EnrollmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuitionApp.Core.Domain.Entities.Timeslot", "Timeslot")
                        .WithMany("Attendances")
                        .HasForeignKey("TimeslotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.ClassSubject", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.CourseClass", "CourseClass")
                        .WithMany("ClassSubjects")
                        .HasForeignKey("CourseClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuitionApp.Core.Domain.Entities.SubjectAssignment", "SubjectAssignment")
                        .WithMany("CourseSubjects")
                        .HasForeignKey("SubjectAssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Classroom", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Location", "Location")
                        .WithMany("Classrooms")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.CourseClass", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Course", "Course")
                        .WithMany("CourseClasses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuitionApp.Core.Domain.Entities.Location", "Location")
                        .WithMany("CourseClasss")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.CourseSubject", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Course", "Course")
                        .WithMany("CourseSubjects")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuitionApp.Core.Domain.Entities.Subject", "Subject")
                        .WithMany("CourseSubjects")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.DailySchedule", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Classroom", "Classroom")
                        .WithMany()
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Enrollment", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.CourseClass", "CourseClass")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuitionApp.Core.Domain.Entities.Student", "Student")
                        .WithMany("Enrollments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.LocationInstructor", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Instructor", "Instructor")
                        .WithMany("LocationInstructors")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuitionApp.Core.Domain.Entities.Location", "Location")
                        .WithMany("LocationInstructors")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.SubjectAssignment", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.Instructor", "Instructor")
                        .WithMany("SubjectAssignments")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuitionApp.Core.Domain.Entities.Subject", "Subject")
                        .WithMany("SubjectAssignments")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuitionApp.Core.Domain.Entities.Timeslot", b =>
                {
                    b.HasOne("TuitionApp.Core.Domain.Entities.ClassSubject", "ClassSubject")
                        .WithMany("Timeslots")
                        .HasForeignKey("ClassSubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuitionApp.Core.Domain.Entities.Classroom", null)
                        .WithMany("Timeslots")
                        .HasForeignKey("ClassroomId");

                    b.HasOne("TuitionApp.Core.Domain.Entities.DailySchedule", "DailySchedule")
                        .WithMany("Timeslots")
                        .HasForeignKey("DailyScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
