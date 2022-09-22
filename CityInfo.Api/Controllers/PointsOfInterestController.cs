using CityInfo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    // /api/cities/3/pointsofinterest
    [Route("api/cities/{cityId}/pointsofinterest")] // dynamic parameter {cityId}
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<PointOfInterestDto> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Instance.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(city.pointOfInterestDtos);
            }
        }

        [HttpGet("{pointOfInterestId}")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Instance.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            else
            {
                var interests = city.pointOfInterestDtos.FirstOrDefault(c => c.Id == pointOfInterestId);
                if (interests == null)
                    return NotFound();

                return Ok(interests);
            }
        }

        #region Format response data: output-formatter, input-formatter
        // Format response data in ASP.NET Core Web API:
        // ref: https://learn.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting?view=aspnetcore-6.0

        // By default, ASP.NET Core supports the following media types:
        //  application/json
        //  text/json
        //  text/plain

        // application/json
        // application/xml

        /* json:
        {
            "id": 5,
            "name": "place3-1",
            "description": "3-1"
        }
        */

        /* SOAP - xml:
        <Person>
            <id>5</id>
            <name>"textname"</name>
            <description>"textdescription"</description>
        </Person>
        */
        #endregion


        /*public IActionResult Index()
        {
            return View();
        }*/
    }
}
