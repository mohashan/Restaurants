
namespace Restaurants.API
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get();
        IEnumerable<WeatherForecast> Generate(int count,int min, int max);
        WeatherForecast Get(int day);
    }
}