using FlightBooking.Database;
using FlightBooking.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        public FlightController() { }
        [HttpGet]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetAsync([FromServices] FlightRepository flightRepository)
        {
            var flights = await flightRepository.GetAllFlights();
            return Ok(flights);
        }

        [Route("search")]
        [HttpPost]
        public async Task<IActionResult> SearchFlight(FlightSearchCriteria criteria, [FromServices] FlightRepository flightRepository)
        {
            var flights = await flightRepository.SearchFlights(criteria);
            return Ok(flights);
        }


        [Route("add-flight")]
        [HttpPost]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> AddFlight(FlightInput flightInput, [FromServices] FlightRepository flightRepository)
        {
            var existing = await flightRepository.GetFlight(x => x.FlightNumber == flightInput.FlightNumber);
            if (existing == null)
            {
                await flightRepository.AddFlight(flightInput);
                return Ok("Flight saved successfully");
            }

            return Conflict(existing);
        }

        [Route("block-unblock-flight/{flightId}/{blocked}")]
        [HttpPut]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> BlockUnblockFlight(int flightId, bool blocked, [FromServices] FlightRepository flightRepository)
        {
            await flightRepository.BlockFlight(flightId, blocked);

            if (blocked)
            {
                return Ok("Flight blocked successfully");
            }
            else
            {
                return Ok("Flight unblocked successfully");
            }

        }
    }
}
