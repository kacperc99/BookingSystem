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
    public class DeskModelsController : ControllerBase
    {
        private readonly BookingSystemContext _context;

        public DeskModelsController(BookingSystemContext context)
        {
            _context = context;
        }

        // GET: api/DeskModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeskModel>>> GetDeskModel()
        {
          if (_context.DeskModel == null)
          {
              return NotFound();
          }
            var desks = await _context.DeskModel.ToListAsync();
      return Ok(desks);
        }

        // GET: api/DeskModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeskModel>> GetDeskModel(int id)
        {
          if (_context.DeskModel == null)
          {
              return NotFound();
          }
            var deskModel = await _context.DeskModel.FindAsync(id);

            if (deskModel == null)
            {
                return NotFound();
            }

            return Ok(deskModel);
        }

        // PUT: api/DeskModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/{deskStatus}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, gigachad")]
    public async Task<IActionResult> PutDeskModel(int id, string deskStatus)
        {
            int? res = _context.DeskModel.Where(e => e.Id == id).Select(e => e.Id).FirstOrDefault();
            var idek = _context.ReservationModel.Where(e=>e.DeskId==id).FirstOrDefault();
            if (res==null || (deskStatus!="available" && deskStatus!="unavailable") || idek!=null)
            {
                return BadRequest("Something got screwed");
            }
      else
      {

        DeskModel record = (from d in _context.DeskModel where d.Id == id select d).FirstOrDefault();
        record.DeskStatus = deskStatus;

        _context.SaveChanges();
        return Ok(record);
      }

            return NoContent();
        }

        // POST: api/DeskModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{deskStatus}/{locationId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, gigachad")]
    public async Task<ActionResult<DeskModel>> PostDeskModel(string deskStatus, int locationId)
        {
          if ((deskStatus != "available" && deskStatus != "unavailable") || locationId == null )
          {
              return Problem("Incorrect or missing values");
          }
          var idd = _context.LocationModel.Where(e => e.Id == locationId).FirstOrDefault();
      if (idd!=null)
      {
        DeskModel desk = new DeskModel();
        desk.DeskStatus = deskStatus;
        desk.LocationID = locationId;
        _context.DeskModel.Add(desk);
        await _context.SaveChangesAsync();
        return desk;
      }
          
            return BadRequest();
        }

    // DELETE: api/DeskModels/5
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, gigachad")]
    public async Task<IActionResult> DeleteDeskModel(int id)
    {
      if (_context.DeskModel == null)
      {
        return NotFound();
      }
      var deskModel = await _context.DeskModel.FindAsync(id);
      if (deskModel == null)
      {
        return NotFound();
      }
      var loc = _context.ReservationModel.Where(e => e.DeskId == id).FirstOrDefault();
      if(loc == null)
      {
        _context.DeskModel.Remove(deskModel);
        await _context.SaveChangesAsync();

        return Ok(deskModel);
      }
      return BadRequest();
    

        }

        private bool DeskModelExists(int id)
        {
            return (_context.DeskModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
