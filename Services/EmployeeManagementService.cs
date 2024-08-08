using Employee.Data;

namespace Employee.Services
{
    public class EmployeeManagementService : IEmployeeManagement
    {
        private readonly EmployeeDbContext _dbContext;

        public EmployeeManagementService(EmployeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        


    }
}
