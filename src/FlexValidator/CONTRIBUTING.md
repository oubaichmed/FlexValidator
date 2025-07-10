# Contributing to FlexValidator

Thank you for your interest in contributing to FlexValidator! We welcome contributions from the community and are pleased to have you aboard.

## 📋 Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Setup](#development-setup)
- [How to Contribute](#how-to-contribute)
- [Coding Standards](#coding-standards)
- [Testing Guidelines](#testing-guidelines)
- [Pull Request Process](#pull-request-process)
- [Issue Guidelines](#issue-guidelines)
- [Documentation](#documentation)
- [Community](#community)

## 🤝 Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code. Please report unacceptable behavior to [mohamedoubaichde@gmail.com](mailto:mohamedoubaichde@gmail.com).

### Our Standards

- **Be respectful** and inclusive
- **Be collaborative** and constructive
- **Be patient** with newcomers
- **Focus on what's best** for the community
- **Show empathy** towards other community members

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Git](https://git-scm.com/)
- IDE of your choice (Visual Studio, VS Code, JetBrains Rider)

### First Time Setup

1. **Fork the repository** on GitHub
2. **Clone your fork** locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/FlexValidator.git
   cd FlexValidator
   ```
3. **Add the upstream remote**:
   ```bash
   git remote add upstream https://github.com/oubaichmed/FlexValidator.git
   ```
4. **Install dependencies**:
   ```bash
   dotnet restore
   ```
5. **Build the project**:
   ```bash
   dotnet build
   ```
6. **Run tests** to ensure everything works:
   ```bash
   dotnet test
   ```

## 🛠️ Development Setup

### Project Structure

```
FlexValidator/
├── src/
│   └── FlexValidator/           # Main library
├── tests/
│   └── FlexValidator.Tests/     # Unit tests
├── samples/
│   └── FlexValidator.Samples/   # Sample applications
├── docs/                        # Documentation
├── .github/                     # GitHub workflows
└── README.md
```

### Building the Project

```bash
# Build in Debug mode
dotnet build

# Build in Release mode
dotnet build --configuration Release

# Build and run tests
dotnet test

# Create NuGet package
dotnet pack src/FlexValidator/FlexValidator.csproj --configuration Release
```

## 🎯 How to Contribute

### Types of Contributions

We welcome several types of contributions:

- 🐛 **Bug fixes**
- ✨ **New features**
- 📚 **Documentation improvements**
- 🧪 **Test coverage improvements**
- 🎨 **Code quality improvements**
- 🌍 **Localization/Internationalization**
- 📝 **Examples and samples**

### Contribution Workflow

1. **Check existing issues** or create a new one
2. **Fork and create a branch** from `main`
3. **Make your changes** following our coding standards
4. **Add or update tests** as needed
5. **Update documentation** if required
6. **Ensure all tests pass**
7. **Submit a pull request**

## 📝 Coding Standards

### General Guidelines

- Follow [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)
- Use meaningful and descriptive names
- Write self-documenting code with clear intent
- Keep methods small and focused on a single responsibility
- Use async/await patterns appropriately

### Code Style

We use `.editorconfig` to maintain consistent code style. Key points:

- **Indentation**: Tabs (4 spaces equivalent)
- **Line endings**: CRLF on Windows, LF on Unix
- **Encoding**: UTF-8
- **Trim trailing whitespace**: Yes
- **Insert final newline**: Yes

### Naming Conventions

```csharp
// Classes, Methods, Properties - PascalCase
public class CustomerValidator : AbstractValidator<Customer>
public void ValidateEmail(string email)
public string ErrorMessage { get; set; }

// Private fields - camelCase with underscore prefix
private readonly IValidator _validator;

// Local variables, parameters - camelCase
public void ProcessCustomer(Customer customer)
{
    var validationResult = validator.Validate(customer);
}

// Constants - PascalCase
public const string DefaultErrorMessage = "Validation failed";
```

### Documentation

- Use XML documentation comments for all public APIs
- Include `<summary>`, `<param>`, `<returns>`, and `<example>` tags where appropriate

```csharp
/// <summary>
/// Validates the specified instance using the configured validation rules.
/// </summary>
/// <param name="instance">The instance to validate.</param>
/// <returns>A <see cref="ValidationResult"/> containing any validation failures.</returns>
/// <example>
/// <code>
/// var validator = new CustomerValidator();
/// var result = validator.Validate(customer);
/// if (!result.IsValid)
/// {
///     // Handle validation errors
/// }
/// </code>
/// </example>
public ValidationResult Validate(T instance)
```

## 🧪 Testing Guidelines

### Test Structure

- Place tests in `tests/FlexValidator.Tests/`
- Use xUnit as the testing framework
- Use FluentAssertions for assertions
- Follow the Arrange-Act-Assert pattern

### Test Naming

```csharp
[Fact]
public void Validate_WithValidInput_ShouldReturnValidResult()
{
    // Arrange
    var validator = new CustomerValidator();
    var customer = new Customer { Name = "John", Age = 30 };

    // Act
    var result = validator.Validate(customer);

    // Assert
    result.IsValid.Should().BeTrue();
}
```

### Test Categories

- **Unit Tests**: Test individual components in isolation
- **Integration Tests**: Test component interactions
- **Performance Tests**: Benchmark critical paths

### Coverage Requirements

- Aim for **90%+ code coverage**
- All new public APIs must have tests
- Bug fixes should include regression tests

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test
dotnet test --filter "MethodName=Validate_WithValidInput_ShouldReturnValidResult"
```

## 🔄 Pull Request Process

### Before Submitting

1. **Update your branch** with the latest upstream changes:
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

2. **Ensure all tests pass**:
   ```bash
   dotnet test
   ```

3. **Check code formatting**:
   ```bash
   dotnet format --verify-no-changes
   ```

### PR Requirements

- **Clear title** describing the change
- **Detailed description** explaining what and why
- **Link to related issues** using `Fixes #123` or `Closes #123`
- **Screenshots** for UI changes (if applicable)
- **Breaking changes** clearly documented

### PR Template

```markdown
## Description
Brief description of the change and which issue it fixes.

## Type of Change
- [ ] Bug fix (non-breaking change which fixes an issue)
- [ ] New feature (non-breaking change which adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update

## Testing
- [ ] I have added tests that prove my fix is effective or that my feature works
- [ ] New and existing unit tests pass locally with my changes
- [ ] I have tested this change manually

## Checklist
- [ ] My code follows the code style of this project
- [ ] I have performed a self-review of my own code
- [ ] I have commented my code, particularly in hard-to-understand areas
- [ ] I have made corresponding changes to the documentation
- [ ] My changes generate no new warnings
```

### Review Process

1. **Automated checks** must pass (CI/CD pipeline)
2. **Code review** by at least one maintainer
3. **All feedback** addressed before merge
4. **Squash and merge** for clean history

## 🐛 Issue Guidelines

### Before Creating an Issue

1. **Search existing issues** to avoid duplicates
2. **Check documentation** and samples
3. **Try the latest version** to see if it's already fixed

### Bug Reports

Use the bug report template and include:

- **Clear title** and description
- **Steps to reproduce** the issue
- **Expected vs actual behavior**
- **Environment details** (.NET version, OS, etc.)
- **Minimal code sample** demonstrating the issue

### Feature Requests

Use the feature request template and include:

- **Clear description** of the proposed feature
- **Use case and motivation** for the feature
- **Potential implementation ideas** (if any)
- **Breaking change considerations**

### Issue Labels

- `bug` - Something isn't working
- `enhancement` - New feature or request
- `documentation` - Improvements or additions to documentation
- `good first issue` - Good for newcomers
- `help wanted` - Extra attention is needed
- `breaking change` - Would break existing functionality

## 📚 Documentation

### Types of Documentation

- **API Documentation**: XML comments in code
- **User Guide**: README.md and wiki pages
- **Examples**: Working code samples
- **Architecture**: Design decisions and patterns

### Documentation Standards

- Write in clear, simple English
- Use code examples liberally
- Keep examples up-to-date with API changes
- Include both simple and advanced scenarios

### Building Documentation

```bash
# Generate API documentation
dotnet build --configuration Release

# The XML documentation will be in bin/Release/net8.0/FlexValidator.xml
```

## 🌍 Community

### Getting Help

- **GitHub Discussions**: For questions and general discussion
- **GitHub Issues**: For bug reports and feature requests
- **Stack Overflow**: Tag your questions with `flexvalidator`

### Communication Channels

- **GitHub**: Primary communication platform
- **Email**: [mohamedoubaichdev@gmail.com](mailto:mohamedoubaichde@gmail.com) for private concerns

### Recognition

Contributors are recognized in:
- **README.md**: Contributors section
- **Release notes**: Major contributors mentioned
- **GitHub**: Contributor graph and statistics

## 🎉 Thank You

We appreciate all contributions, no matter how small. Every contribution helps make FlexValidator better for everyone!

### Top Contributors

Special thanks to all our contributors who have helped make FlexValidator what it is today.

---

## 📞 Questions?

If you have any questions about contributing, please:
1. Check this guide first
2. Search existing GitHub discussions
3. Create a new discussion if needed
4. Contact the maintainers directly for sensitive matters

**Happy Contributing!** 🚀
