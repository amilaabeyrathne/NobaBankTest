using System.ComponentModel.DataAnnotations;
using CarRentalSystem.Api.Models.Reservation;
using Xunit;

namespace CarRentalSystem.Tests;

public class ReservationRequestModelTests
{
    [Theory]
    [InlineData("19870215-1234")]
    [InlineData("8702151234")]
    [InlineData("200012310001")]
    [InlineData("200002290001")] 
    public void SSN_ValidPatterns_PassValidation(string ssn)
    {
        var model = new ReservationRequestModel
        {
            CustomerSocialSecurityNumber = ssn,
            CarId = Guid.NewGuid(),
            PickupMeterReading = 1000
        };

        var results = Validate(model);

        Assert.DoesNotContain(results, r => r.MemberNames.Contains(nameof(ReservationRequestModel.CustomerSocialSecurityNumber)));
    }

    [Theory]
    [InlineData("19871301-1234")]  
    [InlineData("19870232-1234")]  
    [InlineData("abcdefg1234")]   
    [InlineData("1987021512345")]  
    [InlineData("1987021")]        
    public void SSN_InvalidPatterns_FailValidation(string ssn)
    {
        var model = new ReservationRequestModel
        {
            CustomerSocialSecurityNumber = ssn,
            CarId = Guid.NewGuid(),
            PickupMeterReading = 1000
        };

        var results = Validate(model);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ReservationRequestModel.CustomerSocialSecurityNumber)));
    }

    private static IList<ValidationResult> Validate(ReservationRequestModel instance)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, new ValidationContext(instance), results, validateAllProperties: true);
        return results;
    }
}




