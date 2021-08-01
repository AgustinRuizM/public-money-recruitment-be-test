using VacationRental.Api.Core.Base.Services;
using VacationRental.Api.Core.Repository;
using VacationRental.Api.Core.Services.Contracts;
using VacationRental.Api.Model;

namespace VacationRental.Api.Core.Services.Implementations
{
    public class RentalService<T> : Service<T>, IRentalService<T> where T : Rental
    {
        public RentalService(IRepository<T> repository) : base(repository)
        {

        }
    }
}
