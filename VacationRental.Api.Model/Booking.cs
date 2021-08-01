﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Api.Model
{
    public class Booking
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
