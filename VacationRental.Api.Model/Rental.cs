using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Api.Model
{
    public class Rental
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
