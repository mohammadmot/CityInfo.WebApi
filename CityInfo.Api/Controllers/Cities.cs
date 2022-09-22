using CityInfo.Api.Models; // CityDto
using Microsoft.AspNetCore.Mvc; // for Controller or ControllerBase

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Route("api/Cities")] // way1 is better: default route path
    // [Route("api/[controller]")] // way2: fill controller with class name: Cities
    public class Cities : ControllerBase
    {
        // action methods ...

        // ---------------------------------
        // [HttpGet("api/cities")]
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(CitiesDataStore.Instance.Cities);
        }

        /*
        [HttpGet]
        public JsonResult GetCities()
        {
            // return new JsonResult(CitiesDataStore.Instance.Cities);    

            return new JsonResult(new List<object> // Anonymous object => standard way: DTO or POCO 
            { 
                new { id = 1, Name = "Tehran" },
                new { id = 2, Name = "Qom" }
            } );
        }
        */

        // ---------------------------------
        /*
        [HttpGet("{id}")]
        public JsonResult GetCity(int id = 1)
        {
            #region status code
            // -----------
            // Level 200 - Success
            // 200 - Ok
            // 201 - Created (ok by server)
            // 204 - No Content (ok by server)
            // -----------
            // Level 400 - Client Mistake
            // 400 - Bad Request
            // 401 - Unauthorized
            // 403 - Forbidden (lgoin but not access)
            // 404 - Not Found (bad address)
            // 409 - Conflict (access to change 1 resource by 2 clients in a same time)
            // -----------
            // Level 500 - Server Mistake
            // 500 - Internal Server Error
            // ...
            // -----------
            #endregion

            // return Ok();

            // return new JsonResult(CitiesDataStore.Instance.Cities.FirstOrDefault(i => i.Id == id));

            var result = new JsonResult(CitiesDataStore.Instance.Cities.FirstOrDefault(i => i.Id == id));
            result.StatusCode = 200;
            return new JsonResult(result);
        }
        */

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<CityDto>> GetCity(int id = 1)
        {
            var results = CitiesDataStore.Instance.Cities.FirstOrDefault(c => c.Id == id);
            // not found the city
            if (results == null)
            {
                return NotFound();
            }

            return Ok(results);
        }
    }
}