using CarRentalSystem.Domain.Entities;
using Xunit;

namespace CarRentalSystem.Tests;

public class CarTests
{
    [Fact]
    public void MarkAsUnavailable_ShouldThrow_WhenAlreadyUnavailable()
    {
        var car = new Car("ABC123", 1, 1000, "Toyota", "Camry", "Blue", isAvailableToRent: false);
       
        Assert.Throws<InvalidOperationException>(() => car.MarkAsUnavailable());
    }
}

