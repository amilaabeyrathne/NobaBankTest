using System;

namespace CarRentalSystem.Api.Models.Cars;

public class CarModel
{
    public Guid Id { get; set; }
    public string RegistrationNumber { get; set; }
    public int CategoryId { get; set; }
    public int Milage { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Colour { get; set; }
}

