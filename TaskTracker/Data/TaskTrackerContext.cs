using Microsoft.EntityFrameworkCore;
using TaskTracker.Models;

namespace TaskTracker.Data
{
    public class TaskTrackerContext : DbContext
    {
        public TaskTrackerContext(DbContextOptions<TaskTrackerContext> options) : base(options)
        {
        }

        // DbSet 屬性
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 設定實體關聯
            
            // Project -> Tasks (一對多)
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Tasks (一對多)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Tasks)
                .WithOne(t => t.AssignedUser)
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 設定索引
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // 設定種子資料
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // 專案種子資料
            modelBuilder.Entity<Project>().HasData(
                new Project 
                { 
                    ProjectId = 1, 
                    ProjectName = "TaskTracker 系統開發", 
                    Description = "開發任務追蹤系統，展示 CRUD 功能", 
                    Status = "進行中",
                    StartDate = DateTime.Now.AddDays(-30),
                    EndDate = DateTime.Now.AddDays(30),
                    CreatedDate = DateTime.Now.AddDays(-30)
                },
                new Project 
                { 
                    ProjectId = 2, 
                    ProjectName = "期末報告準備", 
                    Description = "準備期末報告相關文件和展示", 
                    Status = "進行中",
                    StartDate = DateTime.Now.AddDays(-7),
                    EndDate = DateTime.Now.AddDays(14),
                    CreatedDate = DateTime.Now.AddDays(-7)
                }
            );

            // 使用者種子資料
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    UserId = 1, 
                    UserName = "張小明", 
                    Email = "ming.zhang@example.com", 
                    Department = "開發部",
                    Position = "前端工程師",
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-30)
                },
                new User 
                { 
                    UserId = 2, 
                    UserName = "李小華", 
                    Email = "hua.li@example.com", 
                    Department = "開發部",
                    Position = "後端工程師",
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-30)
                },
                new User 
                { 
                    UserId = 3, 
                    UserName = "王小美", 
                    Email = "mei.wang@example.com", 
                    Department = "設計部",
                    Position = "UI設計師",
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-30)
                }
            );

            // 任務種子資料
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem 
                { 
                    TaskId = 1, 
                    TaskName = "建立資料庫結構", 
                    Description = "設計並建立 SQLite 資料庫", 
                    ProjectId = 1,
                    AssignedUserId = 2,
                    Priority = "高",
                    Status = "已完成",
                    DueDate = DateTime.Now.AddDays(-20),
                    CompletedDate = DateTime.Now.AddDays(-21),
                    CreatedDate = DateTime.Now.AddDays(-25)
                },
                new TaskItem 
                { 
                    TaskId = 2, 
                    TaskName = "實作 CRUD 功能", 
                    Description = "開發專案、使用者和任務的 CRUD 功能", 
                    ProjectId = 1,
                    AssignedUserId = 2,
                    Priority = "高",
                    Status = "進行中",
                    DueDate = DateTime.Now.AddDays(10),
                    CreatedDate = DateTime.Now.AddDays(-20)
                },
                new TaskItem 
                { 
                    TaskId = 3, 
                    TaskName = "設計使用者介面", 
                    Description = "使用 Tailwind CSS 設計現代化 UI", 
                    ProjectId = 1,
                    AssignedUserId = 3,
                    Priority = "中",
                    Status = "待辦",
                    DueDate = DateTime.Now.AddDays(15),
                    CreatedDate = DateTime.Now.AddDays(-15)
                },
                new TaskItem 
                { 
                    TaskId = 4, 
                    TaskName = "準備期末報告投影片", 
                    Description = "製作期末報告展示投影片", 
                    ProjectId = 2,
                    AssignedUserId = 1,
                    Priority = "高",
                    Status = "進行中",
                    DueDate = DateTime.Now.AddDays(10),
                    CreatedDate = DateTime.Now.AddDays(-5)
                }
            );
        }
    }
} 