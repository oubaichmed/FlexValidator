using FluentAssertions;
using Xunit;

namespace FlexValidator.Tests;

public class AbstractValidatorTests
{
    public class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    public class TestModelValidator : AbstractValidator<TestModel>
    {
        public TestModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(x => x.Age)
                .GreaterThan(0)
                .WithMessage("Age must be greater than 0");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required");
        }
    }

    [Fact]
    public void Validate_ValidModel_ShouldReturnValid()
    {
        // Arrange
        var validator = new TestModelValidator();
        var model = new TestModel
        {
            Name = "John Doe",
            Age = 30,
            Email = "john@example.com"
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_InvalidModel_ShouldReturnInvalid()
    {
        // Arrange
        var validator = new TestModelValidator();
        var model = new TestModel
        {
            Name = "",
            Age = -5,
            Email = ""
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
        result.Errors.Should().Contain(x => x.ErrorMessage == "Name is required");
        result.Errors.Should().Contain(x => x.ErrorMessage == "Age must be greater than 0");
        result.Errors.Should().Contain(x => x.ErrorMessage == "Email is required");
    }

    [Fact]
    public async Task ValidateAsync_ValidModel_ShouldReturnValid()
    {
        // Arrange
        var validator = new TestModelValidator();
        var model = new TestModel
        {
            Name = "John Doe",
            Age = 30,
            Email = "john@example.com"
        };

        // Act
        var result = await validator.ValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void InlineValidator_ShouldWork()
    {
        // Arrange
        var validator = new InlineValidator<TestModel>();
        validator.RuleFor(x => x.Name).NotEmpty();
        validator.RuleFor(x => x.Age).GreaterThan(0);

        var model = new TestModel
        {
            Name = "John Doe",
            Age = 30,
            Email = "john@example.com"
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}