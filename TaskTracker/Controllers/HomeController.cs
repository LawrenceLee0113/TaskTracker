using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Models;
using TaskTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TaskTracker.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TaskTrackerContext _context;

    public HomeController(ILogger<HomeController> logger, TaskTrackerContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role);
        var isAdmin = currentUserRole == "Admin";

        // 根據使用者角色查詢不同的統計資料
        if (isAdmin)
        {
            // 管理員可以看到所有資料
            var totalProjects = await _context.Projects.CountAsync();
            var totalTasks = await _context.Tasks.CountAsync();
            var totalUsers = await _context.Users.CountAsync(u => u.IsActive);
            var completedTasks = await _context.Tasks.CountAsync(t => t.Status == "已完成");
            var inProgressTasks = await _context.Tasks.CountAsync(t => t.Status == "進行中");
            var pendingTasks = await _context.Tasks.CountAsync(t => t.Status == "待辦");

            // 計算完成率
            var completionRate = totalTasks > 0 ? (completedTasks * 100) / totalTasks : 0;

            // 建立 ViewModel
            var dashboardData = new DashboardViewModel
            {
                TotalProjects = totalProjects,
                TotalTasks = totalTasks,
                TotalUsers = totalUsers,
                CompletedTasks = completedTasks,
                InProgressTasks = inProgressTasks,
                PendingTasks = pendingTasks,
                CompletionRate = completionRate,
                IsAdmin = true,
                CurrentUserName = User.FindFirstValue(ClaimTypes.Name) ?? ""
            };

            return View(dashboardData);
        }
        else
        {
            // 一般使用者只能看到自己的任務統計
            var userId = int.Parse(currentUserId);
            var userTasks = await _context.Tasks
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();

            var totalTasks = userTasks.Count;
            var completedTasks = userTasks.Count(t => t.Status == "已完成");
            var inProgressTasks = userTasks.Count(t => t.Status == "進行中");
            var pendingTasks = userTasks.Count(t => t.Status == "待辦");

            // 計算完成率
            var completionRate = totalTasks > 0 ? (completedTasks * 100) / totalTasks : 0;

            // 取得使用者參與的專案數量
            var userProjects = await _context.Tasks
                .Where(t => t.AssignedUserId == userId)
                .Select(t => t.ProjectId)
                .Distinct()
                .CountAsync();

            var dashboardData = new DashboardViewModel
            {
                TotalProjects = userProjects,
                TotalTasks = totalTasks,
                TotalUsers = 1, // 只顯示自己
                CompletedTasks = completedTasks,
                InProgressTasks = inProgressTasks,
                PendingTasks = pendingTasks,
                CompletionRate = completionRate,
                IsAdmin = false,
                CurrentUserName = User.FindFirstValue(ClaimTypes.Name) ?? ""
            };

            return View(dashboardData);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
