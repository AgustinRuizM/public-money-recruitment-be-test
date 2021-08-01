using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Model;

namespace VacationRental.Api.ViewModels
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }
        public CalendarViewModel(int rentalId, IEnumerable<Booking> bookings, int preparationTimeInDays)
        {
            this.RentalId = rentalId;
            this.Dates = new List<CalendarDateViewModel>();

            this.Dates = bookings.Select(x => x.Start).Distinct().Select(x => new CalendarDateViewModel(x)).OrderBy(x => x.Date).ToList();

            var unit = 1;

            foreach (var b in bookings)
            {
                for (int i = 0; i < b.Nights; i++)
                {
                    var date = this.Dates.FirstOrDefault(x => x.Date == b.Start.AddDays(i));
                    if (date is null) continue;
                    date.Bookings.Add(new CalendarBookingViewModel(b.Id, unit));
                }

                for (int i = b.Nights + 1; i < b.Nights + 1 + preparationTimeInDays; i++)
                {
                    var date = this.Dates.FirstOrDefault(x => x.Date == b.Start.AddDays(i) || x.Date.AddDays(i) == b.Start.AddDays(i));
                    if (date is null) continue;
                    if (this.Dates.FirstOrDefault(x => x.Date == date.Date.AddDays(i)) is null)
                        this.Dates.Add(new CalendarDateViewModel(date.Date.AddDays(i)));

                    this.Dates.FirstOrDefault(x => x.Date == date.Date.AddDays(i)).PreparationTimes.Add(new UnitViewModel(unit));
                }
                unit++;
            }

            this.Dates = this.Dates.OrderBy(x => x.Date).ToList();
        }
    }
}
