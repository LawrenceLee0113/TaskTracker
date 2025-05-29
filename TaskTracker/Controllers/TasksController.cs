using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Models;

namespace TaskTracker.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskTrackerContext _context;

        public TasksController(TaskTrackerContext context)
        {
            _context = context;
        }

        // GET: Tasks (Read - 列表頁) ⭐
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .ToListAsync();
            return View(tasks);
        }

        // GET: Tasks/Details/5 (Read - 詳情頁 + 關聯查詢) ⭐
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(m => m.TaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create (Create - 新增表單頁) ⭐
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
            ViewData["AssignedUserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "UserId", "UserName");
            return View();
        }

        // POST: Tasks/Create (Create - 處理新增) ⭐
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskName,Description,ProjectId,AssignedUserId,Priority,Status,DueDate")] TaskItem task)
        {
            if (ModelState.IsValid)
            {
                task.CreatedDate = DateTime.Now;
                _context.Add(task);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "任務建立成功！";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", task.ProjectId);
            ViewData["AssignedUserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "UserId", "UserName", task.AssignedUserId);
            return View(task);
        }

        // GET: Tasks/Edit/5 (Update - 編輯表單頁) ⭐
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", task.ProjectId);
            ViewData["AssignedUserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "UserId", "UserName", task.AssignedUserId);
            return View(task);
        }

        // POST: Tasks/Edit/5 (Update - 處理編輯) ⭐
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,TaskName,Description,ProjectId,AssignedUserId,Priority,Status,DueDate,CreatedDate,CompletedDate")] TaskItem task)
        {
            if (id != task.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 如果狀態改為已完成，自動設定完成日期
                    if (task.Status == "已完成" && task.CompletedDate == null)
                    {
                        task.CompletedDate = DateTime.Now;
                    }
                    // 如果狀態不是已完成，清除完成日期
                    else if (task.Status != "已完成")
                    {
                        task.CompletedDate = null;
                    }

                    _context.Update(task);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "任務更新成功！";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", task.ProjectId);
            ViewData["AssignedUserId"] = new SelectList(_context.Users.Where(u => u.IsActive), "UserId", "UserName", task.AssignedUserId);
            return View(task);
        }

        // GET: Tasks/Delete/5 (Delete - 刪除確認頁) ⭐
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(m => m.TaskId == id);
                
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5 (Delete - 處理刪除) ⭐
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "任務刪除成功！";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
} 