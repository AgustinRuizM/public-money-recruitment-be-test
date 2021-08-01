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
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        public BookingsController(IMapper mapper, IBookingService<Booking> bookingService, IRentalService<Rental> rentalService)
        {
            Mapper = mapper;
            BookingService = bookingService;
            RentalService = rentalService;
        }

        public IMapper Mapper { get; }
        public IBookingService<Booking> BookingService { get; }
        public IRentalService<Rental> RentalService { get; }

        /// <summary>
        /// Retrieves the booking information for the given booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Returns a BookingViewModel object</returns>
        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            try
            {
                var booking = BookingService.Get(bookingId);
                return Mapper.Map<BookingViewModel>(booking);
            }
            catch
            {
                throw new ApplicationException(Messages.Errors.BookingNotFound);
            }
        }

        /// <summary>
        ///  Creates a new booking for the existing rental
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns a ResourceIdViewModel object</returns>
        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException(Messages.Errors.PositiveNights);

            var bookingId = 0;
                
            if (BookingService.IsFree(model.RentalId, model.Start, model.Start.AddDays(model.Nights)))
            {
                bookingId = BookingService.Create(Mapper.Map<Booking>(model));
            }
            else
            {
                Response.StatusCode = 400;
                throw new ApplicationException(Messages.Errors.NotAvailable);
            }

            return new ResourceIdViewModel(bookingId);
        }
    }
}
