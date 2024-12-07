using System.ComponentModel.DataAnnotations;
using THAMCOMVC.Models;
using Xunit;
using System.Linq;
using System.Collections.Generic;

public class UnitTest1
{
    private List<ValidationResult> ValidateModel(User model)
    {
        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }

    [Fact]
    public void UserModel_ValidData_ShouldPassValidation()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PaymentAddress = "123 Elm Street, Springfield",
            Password = "StrongP@ssword1",
            PhoneNumber = "+12345678901"
        };

        // Act
        var validationResults = ValidateModel(user);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void UserModel_MissingRequiredFields_ShouldFailValidation()
    {
        // Arrange
        var user = new User
        {
            FirstName = "j",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PaymentAddress = "123 Elm Street, Springfield",
            Password = "StrongP@ssword1",
            PhoneNumber = "+12345678901"
        };

        // Act
        var validationResults = ValidateModel(user);

        // Assert
        Assert.Contains(validationResults, v => v.MemberNames.Contains("FirstName"));
    }

    [Fact]
    public void UserModel_InvalidEmail_ShouldFailValidation()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "invalid-email",
            PaymentAddress = "123 Elm Street, Springfield",
            Password = "StrongP@ssword1",
            PhoneNumber = "+12345678901"
        };

        // Act
        var validationResults = ValidateModel(user);

        // Assert
        Assert.Contains(validationResults, v => v.MemberNames.Contains("Email"));
    }

    [Fact]
    public void UserModel_InvalidPassword_ShouldFailValidation()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PaymentAddress = "123 Elm Street, Springfield",
            Password = "weakpassword", // Does not meet the password regex
            PhoneNumber = "+12345678901"
        };

        // Act
        var validationResults = ValidateModel(user);

        // Assert
        Assert.Contains(validationResults, v => v.MemberNames.Contains("Password"));
    }

    [Fact]
    public void UserModel_InvalidPhoneNumber_ShouldFailValidation()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PaymentAddress = "123 Elm Street, Springfield",
            Password = "StrongP@ssword1",
            PhoneNumber = "12345" // Invalid phone number format
        };

        // Act
        var validationResults = ValidateModel(user);

        // Assert
        Assert.Contains(validationResults, v => v.MemberNames.Contains("PhoneNumber"));
    }

    [Fact]
    public void UserModel_PaymentAddressTooLong_ShouldFailValidation()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PaymentAddress = new string('a', 501), // Exceeds 500 characters
            Password = "StrongP@ssword1",
            PhoneNumber = "+1234567890"
        };

        // Act
        var validationResults = ValidateModel(user);

        // Assert
        Assert.Contains(validationResults, v => v.MemberNames.Contains("PaymentAddress"));
    }

    
}
