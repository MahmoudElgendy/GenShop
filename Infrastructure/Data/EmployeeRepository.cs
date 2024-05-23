using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;

        public EmployeeRepository(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }


        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeContext.Employees.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _employeeContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task InsertAsync(Employee employee)
        {
            _employeeContext.Employees.Add(employee);
            await _employeeContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _employeeContext.Employees.Update(employee);
            await _employeeContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _employeeContext.Employees.Remove(employee);
            await _employeeContext.SaveChangesAsync();
        }
    }
}
