using Employee.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeRewardManagement.Controllers
{
    [Authorize]

    public class PermissionController : Controller
    {
        private readonly EmployeeDbContext _dbContext;

        public PermissionController(EmployeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Employees = await _dbContext.Employees.Where(x => x.UserId == userId).ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Permission Permission)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var employeeExists = await _dbContext.Employees.Where(e => e.EmployeeId == Permission.EmployeeId && e.UserId == userId).FirstOrDefaultAsync();
                if (employeeExists == null)
                {
                    ModelState.AddModelError("EmployeeId", "Selected employee does not exist.");
                    ViewBag.Employees = await _dbContext.Employees.ToListAsync();
                    return View(Permission);
                }
                Permission.Employee = employeeExists;
                _dbContext.Permissions.Add(Permission);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Employee");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Permission = await _dbContext.Permissions.FindAsync(id);
            if (Permission == null)
            {
                return NotFound();
            }

            return View(Permission);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Permission Permission)
        {
            var CurrentDetails = await _dbContext.Permissions.FindAsync(Permission.PermissionId);

            if (CurrentDetails != null)
            {
                CurrentDetails.PermissionType = Permission.PermissionType;
                CurrentDetails.Status = Permission.Status;
                CurrentDetails.Date = Permission.Date;

                _dbContext.Permissions.Update(CurrentDetails);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Employee");
            }

            return NotFound();
        }


        public async Task<IActionResult> Delete(int id)
        {
            var Permission = await _dbContext.Permissions.FindAsync(id);

            return View(Permission);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Permission = await _dbContext.Permissions.FindAsync(id);

            if (Permission != null)
            {
                _dbContext.Permissions.Remove(Permission);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Employee");
        }

    }
}
