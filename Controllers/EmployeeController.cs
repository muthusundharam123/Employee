using Employee.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeRewardManagement.Controllers
{
    [Authorize] 

    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext _dbContext;

        public EmployeeController(EmployeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employees = await _dbContext.Employees
                .Where(e => e.UserId == userId)
                .Include(e => e.Awards)
                .Include(e => e.Leaves)
                .Include(e => e.Permissions)
                .ToListAsync();

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeDetails(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employees = await _dbContext.Employees
                .Where(e => e.UserId == userId && e.EmployeeId == id)
                .Include(e => e.Awards)
                .Include(e => e.Leaves)
                .Include(e => e.Permissions)
                .FirstOrDefaultAsync();

            return View(employees);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDetails employee)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                employee.UserId = userId;
                _dbContext.Employees.Add(employee);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeDetails employee)
        {

            var CurrentDetails = await _dbContext.Employees.FindAsync(employee.EmployeeId);

            if (CurrentDetails != null)
            {
                CurrentDetails.Name = employee.Name;
                CurrentDetails.Email = employee.Email;
                CurrentDetails.Role = employee.Role;
                CurrentDetails.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _dbContext.Employees.Update(CurrentDetails);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return NotFound();
        }


        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);

            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}
