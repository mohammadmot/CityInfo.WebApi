using CityInfo.Api.Models;

namespace CityInfo.Api
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        // singletone instance from current class
        public static CitiesDataStore Instance { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto() { Id = 1, Name = "Tehran", Description = "this is my city" },
                new CityDto() { Id = 2, Name = "Qom", Description = "this is my qom city" },
                new CityDto() { Id = 3, Name = "Shiraz", Description = "this is my shiraz" },
            };
        }

    }
}
