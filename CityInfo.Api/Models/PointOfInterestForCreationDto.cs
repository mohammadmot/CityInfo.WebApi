using System.ComponentModel.DataAnnotations; // data annotations for data validation like: [Required], [ErrorMessage]

namespace CityInfo.Api.Models
{
    public class PointOfInterestForCreationDto
    {
        [Required(ErrorMessage = "Please enter [Name] with max 50 charcters !!!")] // error message for client in api call
        [MaxLength(50)]
        // [System.ComponentModel.DataAnnotations.EmailAddressAttribute]
        // [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("")]
        public string Name { get; set; } = string.Empty;

        // [Required]
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
