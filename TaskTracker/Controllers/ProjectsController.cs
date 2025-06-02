using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace TaskTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProjectsController : Controller
    {
        private readonly TaskTrackerContext _context;

        public ProjectsController(TaskTrackerContext context)
        {
            _context = context;
        }

        // GET: Projects (Read - 列表頁) ⭐
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(p => p.Tasks)
                .ToListAsync();
            return View(projects);
        }

        // GET: Projects/Details/5 (Read - 詳情頁 + 關聯查詢) ⭐
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.AssignedUser)
                .FirstOrDefaultAsync(m => m.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create (Create - 新增表單頁) ⭐
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create (Create - 處理新增) ⭐
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectName,Description,StartDate,EndDate,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.CreatedDate = DateTime.Now;
                _context.Add(project);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "專案建立成功！";
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5 (Update - 編輯表單頁) ⭐
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5 (Update - 處理編輯) ⭐
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ProjectName,Description,StartDate,EndDate,Status,CreatedDate")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "專案更新成功！";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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
            return View(project);
        }

        // GET: Projects/Delete/5 (Delete - 刪除確認頁) ⭐
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
                
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5 (Delete - 處理刪除) ⭐
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "專案刪除成功！";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
} 