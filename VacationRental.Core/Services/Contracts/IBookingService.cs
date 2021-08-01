using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Api.Core.Base.Services;
using VacationRental.Api.Model;

namespace VacationRental.Api.Core.Services.Contracts
{
    public interface IBookingService<T> : IService<T> where T : Booking
    {
        bool IsFree(int rentalId, DateTime start, DateTime end);
        IEnumerable<Booking> GetByRental(int rentalId);
    }
}
