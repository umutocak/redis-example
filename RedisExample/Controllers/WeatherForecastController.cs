using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedisExample.Business.Abstract;

namespace RedisExample.Controllers
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
        private IDistributedCacheManager _distributedCacheManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDistributedCacheManager distributedCacheManager)
        {
            _logger = logger;
            _distributedCacheManager = distributedCacheManager;
        }

        [HttpGet("getall")]
        public List<People> Get()
        {
            if (_distributedCacheManager.Get("peoples") != null)
            {
                return _distributedCacheManager.Get<List<People>>("peoples");
            }
            else
            {
                List<People> peoples = new List<People>();
                peoples.Add(createPeople("Fatih", "Baycu", 22));
                peoples.Add(createPeople("Mehmet", "Berkan", 22));
                _distributedCacheManager.Set("peoples", peoples);
                return _distributedCacheManager.Get<List<People>>("peoples");
            }
        }

        [HttpPost("setRedis")]
        public string Insert()
        {
            _distributedCacheManager.Remove("peoples");
            List<People> peoples = new List<People>();
            peoples.Add(createPeople("Fatih", "Baycu", 22));
            peoples.Add(createPeople("Mehmet", "Berkan", 22));
            peoples.Add(createPeople("Umut", "Ocak", 23));
            _distributedCacheManager.Set("peoples", peoples);
            return "Added";
        }

        [HttpPost("removeRedis")]
        public string Delete()
        {
            _distributedCacheManager.Remove("peoples");
            return "Deleted";
        }

        private People createPeople(string name, string lastName, int age)
        {
            People people = new People();
            people.Name=name;
            people.LastName =lastName;
            people.Age=age;
            return people;
        }
    }
}