using CityInfo.Api.Models;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")] // name of action inside controller
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

        #region Post = Create
        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(
            int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest // extract from body of request
            )
        {
            // json to object => deserialize
            // object to json => serialize

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Instance.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound(); // 404 error
            }

            int nMaxPointOfInterest_Id = CitiesDataStore.Instance.Cities.
                SelectMany(c => c.pointOfInterestDtos)
                .Max(p => p.Id);

            // define new record
            var createPoint = new PointOfInterestDto();
            createPoint.Id = ++nMaxPointOfInterest_Id;              // !!! problem of concurency request !!!
            createPoint.Name = pointOfInterest.Name;                // "+New Interest point [Name]";
            createPoint.Description = pointOfInterest.Description;  // "+New Interest point [Description]";

            // add new record
            city.pointOfInterestDtos.Add(createPoint);

            // created = 201
            return CreatedAtAction("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = createPoint.Id
                },
                createPoint // return new object as a result of api
                );
        }
        #endregion

        #region Edit = Update = Put
        [HttpPut("{pointOfInterestId}")]
        public ActionResult<bool> UpdatePointOfInterest(
            int cityId, 
            int pointOfInterestId,
            PointOfInterestForUpdateDto updatePoint
            )
        {
            var city = CitiesDataStore.Instance.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var point = city.pointOfInterestDtos.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (point == null)
            {
                return NotFound();
            }

            // update => have to fill [all] feilds. even [one] of them need to be update 
            point.Name = updatePoint.Name;
            point.Description = updatePoint.Description;

            return NoContent();
        }

        #endregion

        #region Edit with patch

        #region Http Patch
        // Http Patch, for some of feilds need to update
        // Object Notation (JSON) Patch = Json Patch Document
        // ref: https://www.rfc-editor.org/rfc/rfc6902
        // ref: https://jsonpatch.com/
        //
        // Dotnetcore Package JsonPatch: Microsoft.AspNetCore.JsonPatch
        // Dotnetcore Package NewtonsoftJson: Microsoft.AspNetCore.Mvc.NewtonsoftJson
        //      also add to service collection
        // Obj1 <===> Obj2
        // Obj2.name = Obj1.name // update name
        /* sample of JSON patch:
        [
          { "op": "replace", "path": "/baz",   "value": "boo"     },
          { "op": "add",     "path": "/hello", "value": ["world"] },
          { "op": "remove",  "path": "/foo"                       }
        ]
        */
        #endregion

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartiallyUpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument
            )
        {
            var city = CitiesDataStore.Instance.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();

            // original object
            var pointOriginal = city.pointOfInterestDtos.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOriginal == null)
                return NotFound();

            // copy object from original object to update later
            var pointToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointOriginal.Name,
                Description = pointOriginal.Description,
            };

            /* Sample Message Body:
            [
                {
                "op": "replace",
                "path": "/Name",
                "value": "TestPatchUpdate - Name"
                },
                {
                "op": "replace",
                "path": "/Description",
                "value": "TestPatchUpdate - Desc"
                }
            ]
            */

            // check patchDocument for apply new feild
            if (patchDocument != null)
            {
                patchDocument.ApplyTo(pointToPatch, ModelState); // check with model state for data validation

                // check data validation with model state
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // update original record
                pointOriginal.Name = pointToPatch.Name;
                pointOriginal.Description = pointToPatch.Description;

                return NoContent();
            }

            return NoContent();
        }
        #endregion
        



    }
}
