using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutputHelper;

        public PostRentalTests(IntegrationFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _client = fixture.Client;
            this._testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 5,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/vacationrental/rentals", request))
            {
                postResponse.EnsureSuccessStatusCode();
                _testOutputHelper.WriteLine(postResponse.StatusCode.ToString());
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                getResponse.EnsureSuccessStatusCode();

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                request.Units.Should().Be(getResult.Units);
                request.PreparationTimeInDays.Should().Be(getResult.PreparationTimeInDays);
            }
        }
    }
}
