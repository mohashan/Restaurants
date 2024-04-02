using AutoMapper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantProfile:Profile
{
    public RestaurantProfile()
    {
        CreateMap<UpdateRestaurantCommand, Restaurant>();

        CreateMap<CreateRestaurantCommand, Restaurant>()
            .ForMember(d => d.Address, opt => opt.MapFrom(src => new Address
            {
                City = src.City,
                Street = src.Street,
                PostalCode = src.PostalCode,
            }));
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(c => c.City, opt => opt.MapFrom(d => d.Address == null ? null : d.Address.City))
            .ForMember(c => c.Street, opt => opt.MapFrom(d => d.Address == null ? null : d.Address.Street))
            .ForMember(c => c.PostalCode, opt => opt.MapFrom(d => d.Address == null ? null : d.Address.PostalCode));
    }
}
