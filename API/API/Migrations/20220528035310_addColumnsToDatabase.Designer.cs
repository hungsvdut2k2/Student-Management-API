﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220528035310_addColumnsToDatabase")]
    partial class addColumnsToDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("API.Models.DatabaseModels.Classroom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("FacultyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Classrooms");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.CourseClassroom", b =>
                {
                    b.Property<int>("CourseClassroomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseClassroomId"), 1L, 1);

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Schedule")
                        .HasColumnType("datetime2");

                    b.Property<string>("TeacherName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseClassroomId");

                    b.HasIndex("CourseId");

                    b.ToTable("CoursesClassroom");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.CourseClassroomUserInformation", b =>
                {
                    b.Property<int>("CourseClassId")
                        .HasColumnType("int");

                    b.Property<string>("UserInformationId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CourseClassId", "UserInformationId");

                    b.HasIndex("UserInformationId");

                    b.ToTable("CourseClassroomUserInformations");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.CourseEducationalProgram", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("EducationalProgramId")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "EducationalProgramId");

                    b.HasIndex("EducationalProgramId");

                    b.ToTable("CourseEducationalPrograms");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.EducationalProgram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EducationalProgram");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Faculty");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.Score", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CourseClassroomId")
                        .HasColumnType("int");

                    b.Property<double>("ExcerciseRate")
                        .HasColumnType("float");

                    b.Property<double>("ExcerciseScore")
                        .HasColumnType("float");

                    b.Property<double>("FinalTermRate")
                        .HasColumnType("float");

                    b.Property<double>("FinalTermScore")
                        .HasColumnType("float");

                    b.Property<double>("MidTermRate")
                        .HasColumnType("float");

                    b.Property<double>("MidTermScore")
                        .HasColumnType("float");

                    b.Property<string>("UserInformationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CourseClassroomId");

                    b.HasIndex("UserInformationId");

                    b.ToTable("Score");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserInformationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserInformationId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.UserInformation", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ClassroomId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("ClassroomId");

                    b.ToTable("UsersInformation");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.Classroom", b =>
                {
                    b.HasOne("API.Models.DatabaseModels.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.CourseClassroom", b =>
                {
                    b.HasOne("API.Models.DatabaseModels.Course", "Course")
                        .WithMany("CourseClassrooms")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.CourseClassroomUserInformation", b =>
                {
                    b.HasOne("API.Models.DatabaseModels.CourseClassroom", "CourseClassroom")
                        .WithMany("CourseClassroomUserInformation")
                        .HasForeignKey("CourseClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Models.DatabaseModels.UserInformation", "UserInformation")
                        .WithMany("CourseClassroomUserInformation")
                        .HasForeignKey("UserInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CourseClassroom");

                    b.Navigation("UserInformation");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.CourseEducationalProgram", b =>
                {
                    b.HasOne("API.Models.DatabaseModels.EducationalProgram", "EducationalProgram")
                        .WithMany("CourseEducationalProgram")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Models.DatabaseModels.Course", "Course")
                        .WithMany("CourseEducationalProgram")
                        .HasForeignKey("EducationalProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("EducationalProgram");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.Score", b =>
                {
                    b.HasOne("API.Models.DatabaseModels.CourseClassroom", "CourseClassroom")
                        .WithMany()
                        .HasForeignKey("CourseClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Models.DatabaseModels.UserInformation", "UserInformation")
                        .WithMany()
                        .HasForeignKey("UserInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CourseClassroom");

                    b.Navigation("UserInformation");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.User", b =>
                {
                    b.HasOne("API.Models.DatabaseModels.UserInformation", "UserInformation")
                        .WithOne("User")
                        .HasForeignKey("API.Models.DatabaseModels.User", "UserInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserInformation");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.UserInformation", b =>
                {
                    b.HasOne("API.Models.DatabaseModels.Classroom", "Classroom")
                        .WithMany("Students")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classroom");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.Classroom", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.Course", b =>
                {
                    b.Navigation("CourseClassrooms");

                    b.Navigation("CourseEducationalProgram");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.CourseClassroom", b =>
                {
                    b.Navigation("CourseClassroomUserInformation");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.EducationalProgram", b =>
                {
                    b.Navigation("CourseEducationalProgram");
                });

            modelBuilder.Entity("API.Models.DatabaseModels.UserInformation", b =>
                {
                    b.Navigation("CourseClassroomUserInformation");

                    b.Navigation("User")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
