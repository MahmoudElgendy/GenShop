using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public String FirstName { get; set; }
        [MaxLength(30)]
        public String? MiddleName { get; set; }
        [Required]
        [MaxLength(30)]
        public String LastName { get; set; }
        [Required]
        [Range(0, 1000000)]
        public Decimal Salary { get; set; }
        public int? ManagerId { get; set; }
    }
}
