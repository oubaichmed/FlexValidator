namespace FlexValidator.Samples;
public class Customer
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
    public Address? Address { get; set; }
    public List<string> PhoneNumbers { get; set; } = new();
}