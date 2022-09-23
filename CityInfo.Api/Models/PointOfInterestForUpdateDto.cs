using System.ComponentModel.DataAnnotations;

namespace CityInfo.Api.Models
{
    public class PointOfInterestForUpdateDto
    {
        [Required(ErrorMessage = "Please enter [Name] with max 50 charcters !!!")] // error message for client in api call
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
