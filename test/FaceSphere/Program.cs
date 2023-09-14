using FluentValidation;
using FluentValidation.Results;

Customer customer = new Customer();
CustomerValidator validator = new CustomerValidator();

ValidationResult results = validator.Validate(customer);

if (!results.IsValid)
{
    foreach (var failure in results.Errors)
    {
        Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
    }
}