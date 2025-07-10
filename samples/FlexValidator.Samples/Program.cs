namespace FlexValidator.Samples;
public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .Length(2, 50)
            .WithMessage("First name must be between 2 and 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .Length(2, 50)
            .WithMessage("Last name must be between 2 and 50 characters");

        RuleFor(x => x.Age)
            .GreaterThan(0)
            .WithMessage("Age must be greater than 0")
            .LessThan(150)
            .WithMessage("Age must be less than 150");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .Must(BeValidEmail)
            .WithMessage("Please enter a valid email address");

        RuleFor(x => x.Address)
            .NotNull()
            .WithMessage("Address is required")
            .ChildRules(address =>
            {
                address.RuleFor(x => x.Street).NotEmpty().WithMessage("Street is required");
                address.RuleFor(x => x.City).NotEmpty().WithMessage("City is required");
                address.RuleFor(x => x.ZipCode).NotEmpty().WithMessage("Zip code is required");
                address.RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
            });

        RuleForEach(x => x.PhoneNumbers)
            .NotEmpty()
            .WithMessage("Phone number cannot be empty")
            .Must(BeValidPhoneNumber)
            .WithMessage("Please enter a valid phone number");
    }

    private bool BeValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool BeValidPhoneNumber(string phoneNumber)
    {
        return !string.IsNullOrEmpty(phoneNumber) &&
               phoneNumber.All(c => char.IsDigit(c) || c == '-' || c == ' ' || c == '(' || c == ')');
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("FlexValidator Samples");
        Console.WriteLine("====================");

        await RunBasicValidationExample();
        await RunInlineValidatorExample();
        await RunAsyncValidationExample();
    }

    static async Task RunBasicValidationExample()
    {
        Console.WriteLine("\n1. Basic Validation Example:");
        Console.WriteLine("----------------------------");

        var validator = new CustomerValidator();

        // Valid customer
        var validCustomer = new Customer
        {
            FirstName = "John",
            LastName = "Doe",
            Age = 30,
            Email = "john.doe@example.com",
            Address = new Address
            {
                Street = "123 Main St",
                City = "Anytown",
                ZipCode = "12345",
                Country = "USA"
            },
            PhoneNumbers = new List<string> { "555-123-4567", "555-987-6543" }
        };

        var result = validator.Validate(validCustomer);
        Console.WriteLine($"Valid Customer - IsValid: {result.IsValid}");

        // Invalid customer
        var invalidCustomer = new Customer
        {
            FirstName = "",
            LastName = "D",
            Age = -5,
            Email = "invalid-email",
            Address = null,
            PhoneNumbers = new List<string> { "", "invalid@phone" }
        };

        result = validator.Validate(invalidCustomer);
        Console.WriteLine($"Invalid Customer - IsValid: {result.IsValid}");
        Console.WriteLine("Errors:");
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"  - {error.PropertyName}: {error.ErrorMessage}");
        }
    }

    static async Task RunInlineValidatorExample()
    {
        Console.WriteLine("\n2. Inline Validator Example:");
        Console.WriteLine("-----------------------------");

        var validator = new InlineValidator<Customer>();
        validator.RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
        validator.RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
        validator.RuleFor(x => x.Age).GreaterThan(0).WithMessage("Age must be positive");

        var customer = new Customer
        {
            FirstName = "Jane",
            LastName = "Smith",
            Age = 25
        };

        var result = await validator.ValidateAsync(customer);
        Console.WriteLine($"Inline Validation - IsValid: {result.IsValid}");
    }

    static async Task RunAsyncValidationExample()
    {
        Console.WriteLine("\n3. Async Validation Example:");
        Console.WriteLine("-----------------------------");

        var validator = new CustomerValidator();
        var customer = new Customer
        {
            FirstName = "Bob",
            LastName = "Johnson",
            Age = 35,
            Email = "bob.johnson@example.com",
            Address = new Address
            {
                Street = "456 Oak Ave",
                City = "Somewhere",
                ZipCode = "67890",
                Country = "Canada"
            }
        };

        var result = await validator.ValidateAsync(customer);
        Console.WriteLine($"Async Validation - IsValid: {result.IsValid}");
        Console.WriteLine($"Total validation time: {DateTime.Now:HH:mm:ss}");
    }
}