using API.Controllers;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests
{
    public class EmployeesControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepo = new Mock<IEmployeeRepository>();
            _controller = new EmployeesController(_mockMapper.Object, _mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfEmployeeDtos()
        {
            // Arrange

            var employeeDtos = new List<EmployeeDto> {
                new EmployeeDto { Id = 1, FirstName = "Mahmoud", MiddleName = "Ali", LastName = "Elgendi" },
                new EmployeeDto { Id = 2, FirstName = "Omar", MiddleName = "Fuad", LastName = "Tameemi" }
            };

            // _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(employees);
            _mockMapper.Setup(m => m.Map<IEnumerable<EmployeeDto>>(It.IsAny<IEnumerable<Employee>>())).Returns(employeeDtos);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnEmployees = Assert.IsType<List<EmployeeDto>>(okResult.Value);
            Assert.Equal(returnEmployees, employeeDtos);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            int employeeId = 1;
            _mockRepo.Setup(repo => repo.GetByIdAsync(employeeId)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.GetById(employeeId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithEmployeeDto()
        {
            // Arrange
            int employeeId = 1;
            var employee = new Employee { Id = 1, FirstName = "Mahmoud", MiddleName = "Ali", LastName = "Elgendi" };
            var employeeDto = new EmployeeDto { Id = 1, FirstName = "Mahmoud", MiddleName = "Ali", LastName = "Elgendi" };

            _mockRepo.Setup(repo => repo.GetByIdAsync(employeeId)).ReturnsAsync(employee);
            _mockMapper.Setup(m => m.Map<EmployeeDto>(It.IsAny<Employee>())).Returns(employeeDto);

            // Act
            var result = await _controller.GetById(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnEmployee = Assert.IsType<EmployeeDto>(okResult.Value);
            Assert.Equal(employeeId, returnEmployee.Id);
        }

        [Fact]
        public async Task Insert_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var employeeDto = new EmployeeDto { Id = 1, FirstName = "Mahmoud", MiddleName = "Ali", LastName = "Elgendi" };
            var employee = new Employee { Id = 1, FirstName = "Mahmoud", MiddleName = "Ali", LastName = "Elgendi" };

            _mockMapper.Setup(m => m.Map<Employee>(It.IsAny<EmployeeDto>())).Returns(employee);
            _mockRepo.Setup(repo => repo.InsertAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Insert(employeeDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetById", createdAtActionResult.ActionName);
            Assert.Equal(employeeDto.Id, ((EmployeeDto)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var employeeDto = new EmployeeDto { Id = 1, FirstName = "Mahmoud", MiddleName = "Ali", LastName = "Elgendi" };

            // Act
            var result = await _controller.Update(2, employeeDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNoContent()
        {
            // Arrange
            var employeeDto = new EmployeeDto { Id = 1, FirstName = "Mahmoud", MiddleName = "Ali", LastName = "Elgendi" };
            var employee = new Employee { Id = 1, FirstName = "Mahmoud", MiddleName = "Ali", LastName = "Elgendi" };

            _mockMapper.Setup(m => m.Map<Employee>(It.IsAny<EmployeeDto>())).Returns(employee);
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, employeeDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenEmployeeDoesNotExist()
        {
            // Arrange
            int employeeId = 1;
            _mockRepo.Setup(repo => repo.GetByIdAsync(employeeId)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.Delete(employeeId);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            int employeeId = 1;
            var employee = new Employee { Id = 1, FirstName = "Mahmoud", MiddleName = "Ali", LastName = "Elgendi" };

            _mockRepo.Setup(repo => repo.GetByIdAsync(employeeId)).ReturnsAsync(employee);
            _mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(employeeId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
