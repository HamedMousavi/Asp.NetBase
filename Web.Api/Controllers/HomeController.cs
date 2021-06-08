using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web.Api.Logging;


namespace Web.Api.Controllers
{

    [Route("api/[controller]/{id}")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes("application/json", "application/xml")]
    [Produces("application/json", "application/xml")]
    public class HomeController : ControllerBase
    {

        private readonly ILogger logger;


        public HomeController(ILogger logger)
        {
            this.logger = logger;
        }



        /// <summary>
        /// Get some values
        /// </summary>
        /// <returns>available values</returns>
        /// <remarks>
        /// <param name="id"></param>
        /// Here's some description of this **cool** api \
        /// The api description can consist of multiple lines \
        /// Cute, isn't it?!
        /// [
        ///     {
        ///         // Summary does not support markdown!! but it is supported for the remarks, Params, response tag.
        ///     }
        /// ]
        /// </remarks>
        /// <response code="200">returns a value</response>
        /// <response code="400">Bad request, returns nothing</response>
        /// <response code="404">Not Found, returns nothing</response>
        /// <response code="406">Not Allowed, returns nothing</response>
        /// <response code="500">Error, returns nothing</response>
        [HttpGet]
        public ActionResult<string> Get(int id)
        {
            try
            {
                logger.Trace("/api/home");
                if (id <= 0) return BadRequest();
                return "some value";
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
