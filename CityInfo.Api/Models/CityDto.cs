namespace CityInfo.Api.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; } = String.Empty;

        // online calculate list, as readonly prop
        public int? NumberOfPointsOfInterest {
            get 
            {
                return pointOfInterestDtos.Count;
            }
        }

        // list of (point of interest) places
        public ICollection<PointOfInterestDto> pointOfInterestDtos { get; set; }
        = new List<PointOfInterestDto>();
    }
}
