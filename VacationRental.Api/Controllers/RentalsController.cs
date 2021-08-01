using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Core.Services.Contracts;
using VacationRental.Api.Model;
using VacationRental.Api.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {

        public RentalsController(IMapper mapper, IBookingService<Booking> bookingService, IRentalService<Rental> rentalService)
        {
            Mapper = mapper;
            BookingService = bookingService;
            RentalService = rentalService;
        }

        public IMapper Mapper { get; }
        public IBookingService<Booking> BookingService { get; }
        public IRentalService<Rental> RentalService { get; }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            try
            {
                var rental = RentalService.Get(rentalId);
                return Mapper.Map<RentalViewModel>(rental);
            }
            catch
            {
                throw new ApplicationException(Messages.Errors.RentalNotFound);
            }
        }

        [HttpPost]
        [Route("api/v1/rentals")]
        [Route("~/api/v1/vacationrental/rentals")]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var rentalId = RentalService.Create(Mapper.Map<Rental>(model));
            return new ResourceIdViewModel(rentalId);
        }

        [HttpPut]
        [Route("~/api/v1/vacationrental/rentals/{rentalId:int}")]
        public void Put(int rentalId, RentalBindingModel model)
        {
            RentalService.Update(rentalId, Mapper.Map<Rental>(model));

            //If the length of preparation time is changed then it should be updated for all existing bookings. 
            //The request should fail if decreasing the number of units or increasing the length of preparation time will produce overlapping between 
            //existing bookings and/or their preparation times.
            
        }

        //[Route("~/api/v1/vacationrental/rentals")]
        //[HttpPost]
        //public ResourceIdViewModel PostVacation(RentalBindingModel model)
        //{
        //    return Post(model);
        //}
    }
}
