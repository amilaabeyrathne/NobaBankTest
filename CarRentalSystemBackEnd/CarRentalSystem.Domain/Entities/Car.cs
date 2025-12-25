namespace CarRentalSystem.Domain.Entities;

public class Car
{
    public Guid Id { get; private set; }
    public string RegistrationNumber { get; private set; }
    public int CategoryId { get; private set; }
    public int Milage { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public string Colour { get; private set; }
    public bool IsAvailableToRent { get; private set; }

    public Car(
        string registrationNumber,
        int categoryId,
        int milage,
        string brand,
        string model,
        string colour,
        bool isAvailableToRent = true)
    {
        Id = Guid.NewGuid();
        RegistrationNumber = registrationNumber.Trim();
        CategoryId = categoryId;
        Milage = milage;
        Brand = brand.Trim();
        Model = model.Trim();
        Colour = string.IsNullOrWhiteSpace(colour) ? string.Empty : colour.Trim();
        IsAvailableToRent = isAvailableToRent;
    }

    public void MarkAsUnavailable()
    {
        if (!IsAvailableToRent) throw new InvalidOperationException("Car is already not available to rent.");
        IsAvailableToRent = false;
    }

    public void MarkAsAvailable(int milage)
    {
        Milage = milage;
        IsAvailableToRent = true;
    }

}
