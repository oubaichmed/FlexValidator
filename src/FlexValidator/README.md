# FlexValidator

A flexible and powerful validation library for .NET applications with fluent API support.

[![Build Status](https://github.com/oubaichmed/FlexValidator/workflows/CI/badge.svg)](https://github.com/oubaichmed/FlexValidator/actions)
[![NuGet](https://img.shields.io/nuget/v/FlexValidator.svg)](https://www.nuget.org/packages/FlexValidator/)
[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](LICENSE)

## 🚀 Features

- **Fluent API**: Easy-to-use and readable validation rules
- **Async Support**: Full async/await support for validation
- **Extensible**: Easy to extend with custom validators
- **Performance**: Optimized for high-performance scenarios
- **Localization**: Support for custom error messages and localization
- **Integration**: Works seamlessly with ASP.NET Core, Blazor, and other .NET frameworks

## 📦 Installation

Install via NuGet Package Manager:

```bash
Install-Package FlexValidator
```

Or via .NET CLI:

```bash
dotnet add package FlexValidator
```

## 🎯 Quick Start

### Basic Usage

```csharp
using FlexValidator;

public class Customer
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
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

// Usage
var validator = new CustomerValidator();
var customer = new Customer { Name = "John", Age = 30, Email = "john@example.com" };
var result = validator.Validate(customer);

if (result.IsValid)
{
    Console.WriteLine("Customer is valid!");
}
else
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"Error: {error.ErrorMessage}");
    }
}
```

### Async Validation

```csharp
var result = await validator.ValidateAsync(customer);
```

### Inline Validation

```csharp
var validator = new InlineValidator<Customer>();
validator.RuleFor(x => x.Name).NotEmpty();
validator.RuleFor(x => x.Age).GreaterThan(0);

var result = validator.Validate(customer);
```

## 🔧 Advanced Usage

### Custom Validators

```csharp
public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Email)
            .Must(BeValidEmail)
            .WithMessage("Please enter a valid email address");
    }

    private bool BeValidEmail(string email)
    {
        // Custom email validation logic
        return !string.IsNullOrEmpty(email) && email.Contains("@");
    }
}
```

### Conditional Validation

```csharp
RuleFor(x => x.Age)
    .GreaterThan(18)
    .When(x => x.IsAdult);
```

### Child Object Validation

```csharp
RuleFor(x => x.Address)
    .ChildRules(address =>
    {
        address.RuleFor(x => x.Street).NotEmpty();
        address.RuleFor(x => x.City).NotEmpty();
    });
```

## 🏗️ Integration

### ASP.NET Core

```csharp
// Program.cs
builder.Services.AddScoped<IValidator<Customer>, CustomerValidator>();

// Controller
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IValidator<Customer> _validator;

    public CustomerController(IValidator<Customer> validator)
    {
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
    {
        var result = await _validator.ValidateAsync(customer);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

        // Process valid customer
        return Ok();
    }
}
```

### Blazor

```csharp
@using FlexValidator

<EditForm Model="@customer" OnValidSubmit="@HandleValidSubmit">
    <FlexValidatorValidator />
    <ValidationSummary />
    
    <InputText @bind-Value="customer.Name" />
    <ValidationMessage For="@(() => customer.Name)" />
    
    <button type="submit">Submit</button>
</EditForm>

@code {
    private Customer customer = new();

    private void HandleValidSubmit()
    {
        // Handle valid form submission
    }
}
```

## 📚 Documentation

For comprehensive documentation, please visit our [Wiki](https://github.com/oubaichmed/FlexValidator/wiki).

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for details.

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for your changes
5. Submit a pull request

## 📝 License

This project is licensed under the Apache 2.0 License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Inspired by FluentValidation
- Thanks to all contributors who have helped improve this library

## 📊 Performance

FlexValidator is designed for high-performance scenarios with:
- Compiled expressions for fast property access
- Minimal memory allocations
- Efficient async validation pipeline

## 🔗 Links

- [NuGet Package](https://www.nuget.org/packages/FlexValidator/)
- [GitHub Repository](https://github.com/oubaichmed/FlexValidator)
- [Documentation](https://github.com/oubaichmed/FlexValidator/wiki)
- [Issue Tracker](https://github.com/oubaichmed/FlexValidator/issues)

---

Made with ❤️ by Mohamed OUBAICH
