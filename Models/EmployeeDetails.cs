using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Data
{
    public class EmployeeDetails
    {
        [Key]
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public string UserId { get; set; }
        public ICollection<Award> Awards { get; set; }
        public ICollection<Leave> Leaves { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }


    public class Award
    {
        [Key]
        public int AwardId { get; set; }
        public int EmployeeId { get; set; }
        public string AwardType { get; set; }
        public string Status { get; set; }
        public DateTime AwardDate { get; set; }

        [ForeignKey("EmployeeId")]
        public EmployeeDetails Employee { get; set; }
    }

    public class Leave
    {
        [Key]
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public string LeaveType { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey("EmployeeId")]
        public EmployeeDetails Employee { get; set; }
    }

    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        public int EmployeeId { get; set; }
        public string PermissionType { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("EmployeeId")]
        public EmployeeDetails Employee { get; set; }
    }
}
