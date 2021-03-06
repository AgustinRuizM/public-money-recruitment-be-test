using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.ViewModels;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class GetCalendarTests
    {
        private readonly HttpClient _client;

        public GetCalendarTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/vacationrental/rentals", postRentalRequest))
            {
                postRentalResponse.EnsureSuccessStatusCode();
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 2,
                 Start = new DateTime(2000, 01, 02)
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                postBooking1Response.EnsureSuccessStatusCode();
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2000, 01, 03)
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                postBooking2Response.EnsureSuccessStatusCode();
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights=5"))
            {
                getCalendarResponse.EnsureSuccessStatusCode();

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                postRentalResult.Id.Should().Be(getCalendarResult.RentalId);
                getCalendarResult.Dates.Should().HaveCount(5);
                getCalendarResult.Dates[0].Date.Should().Be(new DateTime(2000, 01, 01));
                getCalendarResult.Dates[0].Bookings.Should().BeEmpty();
                getCalendarResult.Dates[1].Date.Should().Be(new DateTime(2000, 01, 02));
                getCalendarResult.Dates[1].Bookings.Should().ContainSingle();
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);

                getCalendarResult.Dates[2].Date.Should().Be(new DateTime(2000, 01, 03));
                getCalendarResult.Dates[2].Bookings.Should().HaveCount(2);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);

                getCalendarResult.Dates[3].Date.Should().Be(new DateTime(2000, 01, 04));
                getCalendarResult.Dates[3].Bookings.Should().ContainSingle();
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);

                getCalendarResult.Dates[4].Date.Should().Be(new DateTime(2000, 01, 05));
                getCalendarResult.Dates[4].Bookings.Should().BeEmpty();
            }
        }
    }
}
