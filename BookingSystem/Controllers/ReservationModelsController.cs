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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employee, admin, gigachad")]
    public async Task<ActionResult<IEnumerable<ReservationModel>>> GetReservationModel()
        {
      if (_context.ReservationModel == null)
          {
              return NotFound();
          }
      var user = _context.UserModel.Where(e => e.Id == LoggedInUser.Instance.UserId).FirstOrDefault();
      if(user.Rank=="employee")
            return await _context.ReservationModel.Where(e=>e.UserId==LoggedInUser.Instance.UserId).ToListAsync();
      return await _context.ReservationModel.ToListAsync();
    }


        // PUT: api/ReservationModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employee, admin, gigachad")]
    public async Task<IActionResult> PutReservationModel(int id, DateTime? begin, DateTime? end, int? deskId)
        {
             var idek = _context.ReservationModel.Where(e=>e.Id==id).Where(e=>e.UserId==LoggedInUser.Instance.UserId).FirstOrDefault();
      int? deskidek = null;
      if (deskId.HasValue)
      {
        deskidek = _context.DeskModel.Where(e => e.Id == deskId).Where(e => e.DeskStatus == "available").Select(e => e.Id).FirstOrDefault();
        if (!deskidek.HasValue || (idek.BookingBeginDate - DateTime.Now).Days<1)
          return Problem("Desk does not exist or is unavaliable or reservation cannot be edited");

      }
      if (idek != null)
      {
        if(begin!=null && end!=null)
        {
          TimeSpan diff = (TimeSpan)(end - begin);
          if (DateTime.Compare(DateTime.Now, (DateTime)begin) < 0 && DateTime.Compare(DateTime.Now, (DateTime)end) < 0 
            && DateTime.Compare((DateTime)begin, (DateTime)end) < 0 && diff.Days > 1 && diff.Days < 7)
          {
            ReservationModel record = (from d in _context.ReservationModel where d.Id == id select d).FirstOrDefault();
            record.BookingBeginDate = (DateTime)begin;
            record.BookignEndDate = (DateTime)end;
            if (deskidek.HasValue)
            {
              DeskModel olddesk = (from d in _context.DeskModel where d.Id == record.DeskId select d).FirstOrDefault();
              olddesk.DeskStatus = "available";
              record.DeskId = (int)deskId;
            }
            _context.SaveChanges();
            return Ok(record);
          }
          return Problem("Dates are incorrect");
        }
        else if(begin!=null)
        {
          if(DateTime.Compare(DateTime.Now, (DateTime)begin) < 0 && DateTime.Compare((DateTime)begin, idek.BookignEndDate) < 0 
            && (idek.BookignEndDate - (DateTime)begin).Days>1 
            && (idek.BookignEndDate - (DateTime)begin).Days <7)
          {
            ReservationModel record = (from d in _context.ReservationModel where d.Id == id select d).FirstOrDefault();
            record.BookingBeginDate = (DateTime)begin;
            if (deskidek.HasValue)
            {
              DeskModel olddesk = (from d in _context.DeskModel where d.Id == record.DeskId select d).FirstOrDefault();
              olddesk.DeskStatus = "available";
              record.DeskId = (int)deskId;
            }
            _context.SaveChanges();
            return Ok(record);
          }
          return Problem("Dates are incorrect");
        }
        else if (end != null)
        {
          if(DateTime.Compare(DateTime.Now, (DateTime)end) < 0
            && DateTime.Compare(idek.BookingBeginDate, (DateTime)end) < 0 && ((DateTime)end - idek.BookingBeginDate).Days > 1 
            && ((DateTime)end - idek.BookingBeginDate).Days < 7)
          {
            ReservationModel record = (from d in _context.ReservationModel where d.Id == id select d).FirstOrDefault();
            record.BookignEndDate = (DateTime)end;
            if (deskidek.HasValue)
            {
              DeskModel olddesk = (from d in _context.DeskModel where d.Id == record.DeskId select d).FirstOrDefault();
              olddesk.DeskStatus = "available";
              record.DeskId = (int)deskId;
            }
            _context.SaveChanges();
            return Ok(record);
          }
          return Problem("Dates are incorrect");
        }
        else if(deskidek!=null && (idek.BookingBeginDate - DateTime.Now).Days > 1)
        {
          ReservationModel record = (from d in _context.ReservationModel where d.Id == id select d).FirstOrDefault();
          DeskModel olddesk = (from d in _context.DeskModel where d.Id == record.DeskId select d).FirstOrDefault();
          olddesk.DeskStatus = "available";
          record.DeskId = (int)deskId;
          _context.SaveChanges();
          return Ok(record);
        }
        return Problem("Data incorrect");
            }


      return Problem("Error adding records to a database - incorrect ID");
    }



    // POST: api/ReservationModels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("{begin}/{end}/{deskId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employee, admin, gigachad")]
    public async Task<ActionResult<ReservationModel>> PostReservationModel(DateTime begin, DateTime end, int deskId)
    {
      if (begin != null && end != null && deskId != null)
      {
        var diff = end - begin;
        if (DateTime.Compare(DateTime.Now, begin) < 0 && DateTime.Compare(DateTime.Now, end) < 0 && DateTime.Compare(begin, end) < 0 && diff.Days > 1 && diff.Days < 7)
        {
          var check = _context.DeskModel.Where(e => e.Id == deskId).FirstOrDefault();
          if (check != null && check.DeskStatus == "available")
          {
            ReservationModel book = new ReservationModel();
            book.BookingBeginDate = begin;
            book.BookignEndDate = end;
            book.DeskId = deskId;
            book.UserId = LoggedInUser.Instance.UserId;
            _context.ReservationModel.Add(book);
            await _context.SaveChangesAsync();

            DeskModel record = (from d in _context.DeskModel where d.Id == deskId select d).FirstOrDefault();
            record.DeskStatus = "unavailable";
            _context.SaveChanges();
            return Ok(book);
          }
          return Problem("Wrong ID");
        }
        return Problem("Incorrect Date provided");
      }
      return Problem("Null values provided");
    }
        // DELETE: api/ReservationModels/5
        [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employee, admin, gigachad")]
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
      var user = _context.UserModel.Where(e => e.Id == LoggedInUser.Instance.UserId).FirstOrDefault();
      if (user.Rank == "admin" || user.Rank == "gigachad" || reservationModel.UserId==LoggedInUser.Instance.UserId)
      {
              _context.ReservationModel.Remove(reservationModel);
              await _context.SaveChangesAsync();

               return NoContent();
            }
      return Problem("Operation not permitted");
    }
    

        private bool ReservationModelExists(int id)
        {
            return (_context.ReservationModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
