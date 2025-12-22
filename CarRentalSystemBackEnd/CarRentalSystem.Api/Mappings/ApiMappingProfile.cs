using AutoMapper;
using CarRentalSystem.Api.Models.CarCategories;
using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Api.Models.Reservation;
using CarRentalSystem.Application.DTOs;

namespace CarRentalSystem.Api.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        // Car categories
        CreateMap<CarCategoryDto, CarCategoriesModel>();

        // Cars
        CreateMap<CarResponseDto, CarModel>();

        // Reservations
        CreateMap<ReservationRequestModel, ReservationDto>();
        CreateMap<ReturnRequestModel, ReturnDto>();
        CreateMap<ReturnResponseDto, ReturnResponseModel>();
        CreateMap<ReservationResponseDto, ReservationResponseModel>();
    }
}

