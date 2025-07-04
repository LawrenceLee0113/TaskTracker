﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskTracker.Data;

#nullable disable

namespace TaskTracker.Migrations
{
    [DbContext(typeof(TaskTrackerContext))]
    [Migration("20250604033939_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TaskTracker.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("ProjectId");

                    b.ToTable("Projects");

                    b.HasData(
                        new
                        {
                            ProjectId = 1,
                            CreatedDate = new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "開發任務追蹤系統，展示 CRUD 功能",
                            EndDate = new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProjectName = "TaskTracker 系統開發",
                            StartDate = new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Status = "進行中"
                        },
                        new
                        {
                            ProjectId = 2,
                            CreatedDate = new DateTime(2024, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "準備期末報告相關文件和展示",
                            EndDate = new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProjectName = "期末報告準備",
                            StartDate = new DateTime(2024, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Status = "進行中"
                        });
                });

            modelBuilder.Entity("TaskTracker.Models.TaskItem", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskId"));

                    b.Property<int>("AssignedUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CompletedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TaskId");

                    b.HasIndex("AssignedUserId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Tasks");

                    b.HasData(
                        new
                        {
                            TaskId = 1,
                            AssignedUserId = 2,
                            CompletedDate = new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedDate = new DateTime(2024, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "設計並建立 SQLite 資料庫",
                            DueDate = new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Priority = "高",
                            ProjectId = 1,
                            Status = "已完成",
                            TaskName = "建立資料庫結構"
                        },
                        new
                        {
                            TaskId = 2,
                            AssignedUserId = 2,
                            CreatedDate = new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "開發專案、使用者和任務的 CRUD 功能",
                            DueDate = new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Priority = "高",
                            ProjectId = 1,
                            Status = "進行中",
                            TaskName = "實作 CRUD 功能"
                        },
                        new
                        {
                            TaskId = 3,
                            AssignedUserId = 3,
                            CreatedDate = new DateTime(2024, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "使用 Tailwind CSS 設計現代化 UI",
                            DueDate = new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Priority = "中",
                            ProjectId = 1,
                            Status = "待辦",
                            TaskName = "設計使用者介面"
                        },
                        new
                        {
                            TaskId = 4,
                            AssignedUserId = 1,
                            CreatedDate = new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "製作期末報告展示投影片",
                            DueDate = new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Priority = "高",
                            ProjectId = 2,
                            Status = "進行中",
                            TaskName = "準備期末報告投影片"
                        });
                });

            modelBuilder.Entity("TaskTracker.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Department")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Position")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            CreatedDate = new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Department = "開發部",
                            Email = "ming.zhang@example.com",
                            IsActive = true,
                            Password = "password123",
                            Position = "前端工程師",
                            Role = "User",
                            UserName = "張小明"
                        },
                        new
                        {
                            UserId = 2,
                            CreatedDate = new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Department = "開發部",
                            Email = "hua.li@example.com",
                            IsActive = true,
                            Password = "password123",
                            Position = "後端工程師",
                            Role = "User",
                            UserName = "李小華"
                        },
                        new
                        {
                            UserId = 3,
                            CreatedDate = new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Department = "設計部",
                            Email = "mei.wang@example.com",
                            IsActive = true,
                            Password = "password123",
                            Position = "UI設計師",
                            Role = "User",
                            UserName = "王小美"
                        },
                        new
                        {
                            UserId = 4,
                            CreatedDate = new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Department = "IT部",
                            Email = "admin@example.com",
                            IsActive = true,
                            Password = "password123",
                            Position = "系統管理員",
                            Role = "Admin",
                            UserName = "系統管理員"
                        });
                });

            modelBuilder.Entity("TaskTracker.Models.TaskItem", b =>
                {
                    b.HasOne("TaskTracker.Models.User", "AssignedUser")
                        .WithMany("Tasks")
                        .HasForeignKey("AssignedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TaskTracker.Models.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignedUser");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("TaskTracker.Models.Project", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("TaskTracker.Models.User", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
