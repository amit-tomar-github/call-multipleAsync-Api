using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        [HttpGet("first")]
        public async Task<IActionResult> GetFirst()
        {
            throw new Exception("Exception we calling first");
            //Delay the api call for 1 second
            await Task.Delay(1000);
            return Ok(new
            {
                FirstValue = 1345
            });
        }


        [HttpGet("second")]
        public async Task<IActionResult> GetSecond()
        {
            //Delay the api call for 1 second
            await Task.Delay(1000);
            return Ok(new
            {
                SecondValue = 1345
            });
        }

        [HttpGet("third")]
        public async Task<IActionResult> GetThird()
        {
            //Delay the api call for 1 second
            await Task.Delay(1000);
            return Ok(new
            {
                ThirdValue = 1345
            });
        }


        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}