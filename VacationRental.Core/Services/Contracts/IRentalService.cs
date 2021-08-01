using VacationRental.Api.Core.Base.Services;
using VacationRental.Api.Model;

namespace VacationRental.Api.Core.Services.Contracts
{
    public interface IRentalService<T> : IService<T> where T : Rental
    {
    }
}
