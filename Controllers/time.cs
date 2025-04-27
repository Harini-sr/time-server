using Microsoft.AspNetCore.Mvc;

namespace time_application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeController : ControllerBase
    {
        [HttpGet("GetTimeByCountry")]
        public IActionResult GetTimeByCountry(string country)
        {
            try
            {
                // Map country to timezone ID
                var timezoneMapping = new Dictionary<string, string>
                {
                    { "India", "India Standard Time" },
                    { "America", "Eastern Standard Time" },
                    { "Australia", "AUS Eastern Standard Time" },
                    { "UK", "GMT Standard Time" }
                };

                if (!timezoneMapping.ContainsKey(country))
                {
                    return BadRequest($"Unsupported country: {country}");
                }

                var timezoneId = timezoneMapping[country];
                var currentTime = DateTime.UtcNow;
                var selectedZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, selectedZone);

                return Ok(new
                {
                    time = localTime.ToString("hh:mm:ss tt"),
                    date = localTime.ToString("yyyy-MM-dd")
                });
            }
            catch (TimeZoneNotFoundException)
            {
                return BadRequest($"Timezone not found for country: {country}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
