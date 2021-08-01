using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Model;

namespace VacationRental.Api.ViewModels.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Booking, BookingViewModel>().ReverseMap();
            CreateMap<Booking, BookingBindingModel>().ReverseMap();
            CreateMap<Rental, RentalViewModel>().ReverseMap();
            CreateMap<Rental, RentalBindingModel>().ReverseMap();
        }
    }
}
