using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Api.Core.Base.Services;
using VacationRental.Api.Core.Repository;
using VacationRental.Api.Core.Services.Contracts;
using VacationRental.Api.Model;

namespace VacationRental.Api.Core.Services.Implementations
{
    public class BookingService<T> : Service<T>, IBookingService<T> where T : Booking
    {
        public BookingService(IRepository<T> repository, IRentalService<Rental> rentalService) : base(repository)
        {
            RentalService = rentalService;
        }

        public IRentalService<Rental> RentalService { get; }

        public bool IsFree(int rentalId, DateTime start, DateTime end)
        {
            var rental = RentalService.Get(rentalId);
            var bookings = GetByRentalAndDate(rentalId, start, end);
            return rental.Units > bookings.Count();
        }

        public IEnumerable<Booking> GetByRental(int rentalId)
        {
            return GetAll().Where(x => x.RentalId == rentalId);
        }

        public IEnumerable<Booking> GetByRentalAndDate(int rentalId, DateTime start, DateTime end)
        {
            var rental = RentalService.Get(rentalId);
            return GetAll().Where(x => x.RentalId == rentalId &&
                                        x.Start <= end &&
                                        start <= x.Start.AddDays(x.Nights + rental.PreparationTimeInDays));
        }

        public int GetFreeUnit(int rentalId, DateTime start, DateTime end)
        {
            var bookings = GetByRentalAndDate(rentalId, start, end);
            if (bookings.Any())
                return bookings.Max(x => x.Unit) + 1;
            else return 1;
        }
    }
}
