using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Models;
using TaskTracker.ViewModels;

namespace TaskTracker.Controllers;

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
        // 查詢實際統計資料
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
            CompletionRate = completionRate
        };

        return View(dashboardData);
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
