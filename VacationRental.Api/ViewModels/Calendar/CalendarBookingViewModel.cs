namespace VacationRental.Api.ViewModels
{
    public class CalendarBookingViewModel
    {
        public CalendarBookingViewModel(int id, int unit)
        {
            this.Id = id;
            this.Unit = unit;
        }
        public int Id { get; set; }
        public int Unit { get; set; }
    }
}
