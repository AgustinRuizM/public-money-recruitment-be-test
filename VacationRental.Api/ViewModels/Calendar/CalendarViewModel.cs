using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Model;

namespace VacationRental.Api.ViewModels
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }

        public CalendarViewModel() { }

        public CalendarViewModel(int rentalId, IEnumerable<Booking> bookings, int preparationTimeInDays, DateTime start, int nights)
        {
            this.RentalId = rentalId;
            this.Dates = new List<CalendarDateViewModel>();
            var end = start.AddDays(nights - 1);
            var dateList = Enumerable.Range(0, 1 + end.Subtract(start).Days).Select(offset => start.AddDays(offset));
            this.Dates = dateList.Select(x => new CalendarDateViewModel(x)).ToList();

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

                    this.Dates.FirstOrDefault(x => x.Date == date.Date.AddDays(i))?.PreparationTimes.Add(new UnitViewModel(unit));
                }
                unit++;
            }

            this.Dates = this.Dates.OrderBy(x => x.Date).ToList();
        }
    }
}
