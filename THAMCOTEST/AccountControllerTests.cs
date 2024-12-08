using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using THAMCOMVC.Controllers;
using THAMCOMVC.Models;
using THAMCOMVC.Data;
using THAMCOMVC.Repositories;
using THAMCOMVC.Interfaces;

namespace THAMCOTEST
{
    public class AccountControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IAccountRepository> _mockRepo;
        private readonly AccountController _controller;

        

        public AccountControllerTests()
        {
            // Mock IConfiguration
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(config => config["Auth0:ClientId"]).Returns("dummy-client-id");
            _mockConfig.Setup(config => config["Auth0:ClientSecret"]).Returns("dummy-client-secret");
            _mockConfig.Setup(config => config["Auth0:Audience"]).Returns("dummy-audience");

            // Mock IAccountRepository
            _mockRepo = new Mock<IAccountRepository>();
            _mockRepo.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            _controller = new AccountController(_mockRepo.Object, _mockConfig.Object, true);
        }


        [Fact]
        public async Task Create_ValidUser_RedirectsToLogin()
        {
            // Arrange
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PaymentAddress = "123 Street",
                Password = "Password@1",
                PhoneNumber = "+12345678901"
            };

            _mockRepo.Setup(repo => repo.AddUserAsync(user)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(user);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
            _mockRepo.Verify(repo => repo.AddUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task Edit_InvalidUserId_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.Edit(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditField_ValidPhoneNumber_UpdatesPhoneNumber()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PaymentAddress = "123 Street",
                Password = "Password@1",
                PhoneNumber = "+12345678901"
            };
            _mockRepo.Setup(repo => repo.GetUserByIdAsync(1)).ReturnsAsync(user);
            _mockRepo.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.EditField(user.Id, "PhoneNumber", "+12345678902");

            // Assert
            Assert.Equal("+12345678902", user.PhoneNumber);
            Assert.IsType<RedirectToActionResult>(result);
            _mockRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        

        [Fact]
        public async Task EditField_InvalidField_ReturnsError()
        {
            // Arrange
            var user = new User { Id = 1,
            FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PaymentAddress = "123 Street",
                Password = "Password@1",
                PhoneNumber = "1238901",
                Auth0UserId = "auth0|12345" };
            _mockRepo.Setup(repo => repo.GetUserByIdAsync(user.Id)).ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.EditField(user.Id, "InvalidField", "value"));
        }
    }
}
