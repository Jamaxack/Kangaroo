using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pricing.API.DataTransferableObjects;
using Pricing.API.Infrastucture.Exceptions;
using Pricing.API.Infrastucture.Repositories;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.API.Infrastucture.Services
{
    public class PricingService : IPricingService
    {
        readonly IPricingRepository _pricingRepository;
        readonly IHttpClientFactory _clientFactory;
        readonly IConfiguration _configuration;

        public PricingService(IPricingRepository pricingRepository, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _pricingRepository = pricingRepository;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public async Task<PriceDto> CalculatePriceAsync(CalculatePriceDto calculatePriceDto)
        {
            var pickUpAddress = calculatePriceDto.PickUpLocation.Address;
            var dropOffAddress = calculatePriceDto.DropOffLocation.Address;
            var key = $"{pickUpAddress}|{dropOffAddress}";
            var pricing = await _pricingRepository.GetPricingAsync(key);

            if (pricing == null)
            {
                var response = await MakeRequest(pickUpAddress, dropOffAddress);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var metrix = JsonConvert.DeserializeObject<DistanceMetrixDto>(responseString);
                    if (metrix.Status.ToUpper() != HttpStatusCode.OK.ToString())
                        throw new PricingDomainException(metrix.Error_Message);

                    var row = metrix.Rows.FirstOrDefault();
                    if (row == null)
                        throw new PricingDomainException("Please try again with different address");

                    var element = row.Elements.FirstOrDefault();
                    if (element == null)
                        throw new PricingDomainException("Please try again with different address");

                    var distanceKilometers = element.Distance.Value / 1000; //to kilometers
                    var durationMinutes = element.Duration.Value / 60; //to minutes
                    var calculatedPrice = distanceKilometers + (durationMinutes / 10) + (calculatePriceDto.Weight / 10); // simple price calculation formula

                    pricing = new Model.Pricing { Price = calculatedPrice, Distance = distanceKilometers, Duration = durationMinutes };
                    await _pricingRepository.InsertPricingAsync(key, pricing);
                }
                else
                    throw new PricingDomainException(await response.Content.ReadAsStringAsync());
            }

            return MapToPriceDto(pricing);
        }

        Task<HttpResponseMessage> MakeRequest(string pickUpAddress, string dropOffAddress)
        {
            var client = _clientFactory.CreateClient();
            var requestUrl = new StringBuilder("https://maps.googleapis.com/maps/api/distancematrix/json");
            requestUrl.Append($"?origins={pickUpAddress}");
            requestUrl.Append($"&destinations={dropOffAddress}");
            requestUrl.Append("&departure_time=now");
            requestUrl.Append($"&key={_configuration["GoogleApiKey"]}"); // stored in secrets

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl.ToString());
            return client.SendAsync(request);
        }

        PriceDto MapToPriceDto(Model.Pricing pricing)
            => new PriceDto
            {
                Price = pricing.Price,
                Distance = pricing.Distance,
                Duration = pricing.Duration
            };
    }
}
