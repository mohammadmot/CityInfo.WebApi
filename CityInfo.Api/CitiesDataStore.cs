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
                new CityDto() { Id = 1, Name = "Tehran", Description = "this is my city",
                    pointOfInterestDtos = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "place1-1",
                            Description = "1-1"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "place1-2",
                            Description = "1-2"
                        },
                    } 
                },
                new CityDto() { Id = 2, Name = "Qom", Description = "this is my qom city",
                    pointOfInterestDtos = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 3,
                            Name = "place2-1",
                            Description = "2-1"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 4,
                            Name = "place2-2",
                            Description = "2-2"
                        },
                    }
                },
                new CityDto() { Id = 3, Name = "Shiraz", Description = "this is my shiraz",
                    pointOfInterestDtos = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 5,
                            Name = "place3-1",
                            Description = "3-1"
                        },
                    }
                },
            };
        }

    }
}
