﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrainingRecordSystemAPI.Data;

#nullable disable

namespace TrainingRecordSystemAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240722063653_altertables")]
    partial class altertables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TrainingRecordSystemAPI.Dtos.DaterangeBasedReportDto", b =>
                {
                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModePreference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TopicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalParticipateNo")
                        .HasColumnType("int");

                    b.Property<string>("TrainerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToView(null);
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Dtos.ModeOfPrefrenceDto", b =>
                {
                    b.Property<string>("ModePreference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToView(null);
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Dtos.MonthlyAdminReportDto", b =>
                {
                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModePreference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TopicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalParticipateNo")
                        .HasColumnType("int");

                    b.ToView(null);
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.Job", b =>
                {
                    b.Property<int>("JobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("JobId"), 1L, 1);

                    b.Property<string>("JobName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("JobId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.ManagerReport", b =>
                {
                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("JobName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TopicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrainerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToView(null);
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.Nomination", b =>
                {
                    b.Property<int>("NominationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NominationId"), 1L, 1);

                    b.Property<string>("ModePreference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParticipateId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.Property<int>("TrainerId")
                        .HasColumnType("int");

                    b.HasKey("NominationId");

                    b.HasIndex("ParticipateId");

                    b.HasIndex("TopicId");

                    b.HasIndex("TrainerId");

                    b.ToTable("Nominations");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.Participate", b =>
                {
                    b.Property<int>("ParticipateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParticipateId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("JobId")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ParticipateId");

                    b.HasIndex("JobId");

                    b.HasIndex("UserId");

                    b.ToTable("Participates");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.Topic", b =>
                {
                    b.Property<int>("TopicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TopicId"), 1L, 1);

                    b.Property<int>("JobId")
                        .HasColumnType("int");

                    b.Property<string>("TopicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TopicId");

                    b.HasIndex("JobId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.TrainerProgramDetail", b =>
                {
                    b.Property<int>("TrainerProgramDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TrainerProgramDetailId"), 1L, 1);

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModePreference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("TargetAudience")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrainerTopicId")
                        .HasColumnType("int");

                    b.HasKey("TrainerProgramDetailId");

                    b.HasIndex("TrainerTopicId");

                    b.ToTable("TrainerProgramDetails");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.TrainerTopic", b =>
                {
                    b.Property<int>("TrainerTopicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TrainerTopicId"), 1L, 1);

                    b.Property<int>("JobId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TrainerTopicId");

                    b.HasIndex("JobId");

                    b.HasIndex("TopicId");

                    b.HasIndex("UserId");

                    b.ToTable("TrainerTopics");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("JobId")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LoginId")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<bool>("Loginbit")
                        .HasColumnType("bit");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("JobId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.Nomination", b =>
                {
                    b.HasOne("TrainingRecordSystemAPI.Models.Participate", "Participate")
                        .WithMany("Nominations")
                        .HasForeignKey("ParticipateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainingRecordSystemAPI.Models.Topic", "Topic")
                        .WithMany("Nominations")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainingRecordSystemAPI.Models.User", "User")
                        .WithMany("Nominations")
                        .HasForeignKey("TrainerId")
                        .IsRequired();

                    b.Navigation("Participate");

                    b.Navigation("Topic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.Participate", b =>
                {
                    b.HasOne("TrainingRecordSystemAPI.Models.Job", "Job")
                        .WithMany("Participates")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainingRecordSystemAPI.Models.User", "User")
                        .WithMany("Participates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.Topic", b =>
                {
                    b.HasOne("TrainingRecordSystemAPI.Models.Job", "Job")
                        .WithMany("Topics")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.TrainerProgramDetail", b =>
                {
                    b.HasOne("TrainingRecordSystemAPI.Models.TrainerTopic", "TrainerTopic")
                        .WithMany("TrainerProgramDetail")
                        .HasForeignKey("TrainerTopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainerTopic");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.TrainerTopic", b =>
                {
                    b.HasOne("TrainingRecordSystemAPI.Models.Job", "Job")
                        .WithMany("TrainerTopics")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainingRecordSystemAPI.Models.Topic", "Topic")
                        .WithMany("TrainerTopics")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainingRecordSystemAPI.Models.User", "User")
                        .WithMany("TrainerTopics")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");

                    b.Navigation("Topic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.User", b =>
                {
                    b.HasOne("TrainingRecordSystemAPI.Models.Job", "Job")
                        .WithMany("Users")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.Job", b =>
                {
                    b.Navigation("Participates");

                    b.Navigation("Topics");

                    b.Navigation("TrainerTopics");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.Participate", b =>
                {
                    b.Navigation("Nominations");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.Topic", b =>
                {
                    b.Navigation("Nominations");

                    b.Navigation("TrainerTopics");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.TrainerTopic", b =>
                {
                    b.Navigation("TrainerProgramDetail");
                });

            modelBuilder.Entity("TrainingRecordSystemAPI.Models.User", b =>
                {
                    b.Navigation("Nominations");

                    b.Navigation("Participates");

                    b.Navigation("TrainerTopics");
                });
#pragma warning restore 612, 618
        }
    }
}
