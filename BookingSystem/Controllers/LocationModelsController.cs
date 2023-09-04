using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Data;
using BookingSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationModelsController : ControllerBase
    {
        private readonly BookingSystemContext _context;

        public LocationModelsController(BookingSystemContext context)
        {
            _context = context;
        }

        // GET: api/LocationModels
        [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employee, admin, gigachad")]
    public async Task<ActionResult<IEnumerable<LocationModel>>> GetLocationModel()
        {
          if (_context.LocationModel == null)
          {
              return NotFound();
          }
            return await _context.LocationModel.ToListAsync();
        }

    // POST: api/LocationModels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, gigachad")]
    public async Task<ActionResult<LocationModel>> PostLocationModel(string locationName)
        {
      if (!string.IsNullOrEmpty(locationName))
      {
        string nam = _context.LocationModel.Where(e => e.Location_Name == locationName).Select(e=>e.Location_Name).FirstOrDefault();
        if (nam == null)
        {
          LocationModel loc = new LocationModel();
          loc.Location_Name = locationName;
          _context.LocationModel.Add(loc);
          await _context.SaveChangesAsync();

          return Ok(loc);
        }
        return BadRequest();

      }
      return NoContent();
    }

    // DELETE: api/LocationModels/5
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, gigachad")]
    public async Task<IActionResult> DeleteLocationModel(int id)
        {
            if (_context.LocationModel == null)
            {
                return NotFound();
            }
            var locationModel = await _context.LocationModel.FindAsync(id);
            if (locationModel == null)
            {
                return NotFound();
            }
            var loc = _context.DeskModel.Where(e=>e.LocationID == id ).FirstOrDefault();
      if (loc == null)
      {
        _context.LocationModel.Remove(locationModel);
        await _context.SaveChangesAsync();

        return NoContent();
      }
      return BadRequest(); 
        }

        private bool LocationModelExists(int id)
        {
            return (_context.LocationModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
