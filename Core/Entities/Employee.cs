namespace Core.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String? MiddleName { get; set; }
        public String LastName { get; set; }
        public Decimal Salary { get; set; }
        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }
    }
}
