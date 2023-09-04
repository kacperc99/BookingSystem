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
    public class ReservationModelsController : ControllerBase
    {
        private readonly BookingSystemContext _context;

        public ReservationModelsController(BookingSystemContext context)
        {
            _context = context;
        }

        // GET: api/ReservationModels
        [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, gigachad")]
    public async Task<ActionResult<IEnumerable<ReservationModel>>> GetReservationModel()
        {
          if (_context.ReservationModel == null)
          {
              return NotFound();
          }
            return await _context.ReservationModel.ToListAsync();
        }

        // GET: api/ReservationModels/5
        [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, gigachad")]
    public async Task<ActionResult<ReservationModel>> GetReservationModel(int id)
        {
          if (_context.ReservationModel == null)
          {
              return NotFound();
          }
            var reservationModel = await _context.ReservationModel.FindAsync(id);

            if (reservationModel == null)
            {
                return NotFound();
            }

            return reservationModel;
        }

        // PUT: api/ReservationModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservationModel(int id, ReservationModel reservationModel)
        {
            if (id != reservationModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(reservationModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ReservationModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReservationModel>> PostReservationModel(ReservationModel reservationModel)
        {
          if (_context.ReservationModel == null)
          {
              return Problem("Entity set 'BookingSystemContext.ReservationModel'  is null.");
          }
            _context.ReservationModel.Add(reservationModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservationModel", new { id = reservationModel.Id }, reservationModel);
        }

        // DELETE: api/ReservationModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservationModel(int id)
        {
            if (_context.ReservationModel == null)
            {
                return NotFound();
            }
            var reservationModel = await _context.ReservationModel.FindAsync(id);
            if (reservationModel == null)
            {
                return NotFound();
            }

            _context.ReservationModel.Remove(reservationModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationModelExists(int id)
        {
            return (_context.ReservationModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
