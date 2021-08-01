using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Core.Services.Contracts;
using VacationRental.Api.Model;
using VacationRental.Api.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        public CalendarController(IMapper mapper, IBookingService<Booking> bookingService, IRentalService<Rental> rentalService)
        {
            Mapper = mapper;
            BookingService = bookingService;
            RentalService = rentalService;
        }

        public IMapper Mapper { get; }
        public IBookingService<Booking> BookingService { get; }
        public IRentalService<Rental> RentalService { get; }

        /// <summary>
        /// Retrieve the booking information for the given query parameters
        /// </summary>
        /// <param name="rentalId"></param>
        /// <param name="start"></param>
        /// <param name="nights"></param>
        /// <returns>Returns a CalendarViewModel object</returns>
        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException(Messages.Errors.PositiveNights);
            var rental = RentalService.Get(rentalId);
            if (rental is null)
                throw new ApplicationException(Messages.Errors.RentalNotFound);

            var bookings = BookingService.GetByRental(rentalId).Where(x => x.Start > start);

            return new CalendarViewModel(rentalId, bookings, rental.PreparationTimeInDays);
        }
    }
}
