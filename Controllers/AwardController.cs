using Employee.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeRewardManagement.Controllers
{
    [Authorize]

    public class AwardController : Controller
    {
        private readonly EmployeeDbContext _dbContext;

        public AwardController(EmployeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Employees = await _dbContext.Employees.Where(e => e.UserId == userId).ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Award award)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                var employeeExists = await _dbContext.Employees.Where(e => e.EmployeeId == award.EmployeeId && e.UserId == userId).FirstOrDefaultAsync();
                if (employeeExists == null)
                {
                    ModelState.AddModelError("EmployeeId", "Selected employee does not exist.");
                    ViewBag.Employees = await _dbContext.Employees.ToListAsync();
                    return View(award);
                }
                award.Employee = employeeExists;

                _dbContext.Awards.Add(award);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Employee");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Award = await _dbContext.Awards.FindAsync(id);
            if (Award == null)
            {
                return NotFound();
            }

            return View(Award);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Award Award)
        {
            var CurrentDetails = await _dbContext.Awards.FindAsync(Award.AwardId);

            if (CurrentDetails != null)
            {
                CurrentDetails.AwardType = Award.AwardType;
                CurrentDetails.Status = Award.Status;
                CurrentDetails.AwardDate = Award.AwardDate;

                _dbContext.Awards.Update(CurrentDetails);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Employee");
            }

            return NotFound();
        }


        public async Task<IActionResult> Delete(int id)
        {
            var Award = await _dbContext.Awards.FindAsync(id);

            return View(Award);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Award = await _dbContext.Awards.FindAsync(id);

            if (Award != null)
            {
                _dbContext.Awards.Remove(Award);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Employee");
        }

    }
}
