using Employee.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeRewardManagement.Controllers
{
    [Authorize]

    public class LeaveController : Controller
    {
        private readonly EmployeeDbContext _dbContext;

        public LeaveController(EmployeeDbContext dbContext)
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
        public async Task<IActionResult> Create(Leave Leave)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var employeeExists = await _dbContext.Employees.Where(e => e.EmployeeId == Leave.EmployeeId && e.UserId == userId).FirstOrDefaultAsync();
                if (employeeExists == null)
                {
                    ModelState.AddModelError("EmployeeId", "Selected employee does not exist.");
                    ViewBag.Employees = await _dbContext.Employees.ToListAsync();
                    return View(Leave);
                }
                Leave.Employee = employeeExists;
                _dbContext.Leaves.Add(Leave);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Employee");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Leave = await _dbContext.Leaves.FindAsync(id);
            if (Leave == null)
            {
                return NotFound();
            }

            return View(Leave);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Leave Leave)
        {
            var CurrentDetails = await _dbContext.Leaves.FindAsync(Leave.LeaveId);

            if (CurrentDetails != null)
            {
                CurrentDetails.LeaveType = Leave.LeaveType;
                CurrentDetails.Status = Leave.Status;
                CurrentDetails.StartDate = Leave.StartDate;
                CurrentDetails.EndDate = Leave.EndDate;

                _dbContext.Leaves.Update(CurrentDetails);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Employee");
            }

            return NotFound();
        }


        public async Task<IActionResult> Delete(int id)
        {
            var Leave = await _dbContext.Leaves.FindAsync(id);

            return View(Leave);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Leave = await _dbContext.Leaves.FindAsync(id);

            if (Leave != null)
            {
                _dbContext.Leaves.Remove(Leave);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Employee");
        }

    }
}
