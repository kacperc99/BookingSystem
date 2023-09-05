using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Data;
using BookingSystem.Model;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BookingSystem.Controllers
{

    
    [Route("api/[controller]")]
    [ApiController]
    public class UserModelsController : ControllerBase
    {
        private readonly BookingSystemContext _context;
        private readonly IConfiguration _configuration;

        public UserModelsController(BookingSystemContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }

        // GET: api/UserModels
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUserModel()
        {
          if (_context.UserModel == null)
          {
              return NotFound();
          }
            return await _context.UserModel.ToListAsync();
        }

        // GET: api/UserModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserModel(int id)
        {
          if (_context.UserModel == null)
          {
              return NotFound();
          }
            var userModel = await _context.UserModel.FindAsync(id);

            if (userModel == null)
            {
                return NotFound();
            }

            return userModel;
        }*/
        [HttpGet("{username}/{password}")]
        public async Task<ActionResult<UserModel>> GetUserModel(string username, string password)
        {
          var usr =  _context.UserModel.Where(e => e.Username == username).Where(e=>e.Password == password).FirstOrDefault();
          if (usr == null)
            return BadRequest("Invalid credentials");
          else
          {
            var claims = new[]
            {
              new Claim(ClaimTypes.NameIdentifier, usr.Username),
              new Claim(ClaimTypes.Role, usr.Rank)
            };
            var token = new JwtSecurityToken
            (
              issuer: _configuration["Jwt:Issuer"],
              audience: _configuration["Jwt:Audience"],
              claims: claims,
              expires:DateTime.UtcNow.AddMinutes(60),
              signingCredentials: new SigningCredentials(
              new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
              SecurityAlgorithms.HmacSha256)
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        LoggedInUser.Instance.UserId = _context.UserModel.Where(e=>e.Username==username).Select(e=>e.Id).FirstOrDefault();
        List<int> desks = _context.ReservationModel.Where(x => x.BookignEndDate < DateTime.Now).Select(e=>e.DeskId).ToList();
        foreach(var desk in desks)
        {
          DeskModel deskModel = (from x in _context.DeskModel where x.Id==desk select x).FirstOrDefault();
          deskModel.DeskStatus = "available";
        }
        _context.ReservationModel.RemoveRange(_context.ReservationModel.Where(x => x.BookignEndDate < DateTime.Now));
        await _context.SaveChangesAsync();
        return Ok(tokenString);
          }
          
        }


    // PUT: api/UserModels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "gigachad")]
    [HttpPut("{id}")]
        public async Task<IActionResult> PutUserModel(int id, UserModel userModel)
        {
            if (id != userModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelExists(id))
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

    // POST: api/UserModels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    
    [HttpPost("{username}/{password}/{passwordrepeat}")]
        public async Task<ActionResult<UserModel>> PostUserModel(string username, string password, string passwordrepeat)
        {
            if(!string.IsNullOrEmpty(username)&& !string.IsNullOrEmpty(password)&& !string.IsNullOrEmpty(passwordrepeat))
            {
                 var usr = _context.UserModel.Where(e => e.Username == username).FirstOrDefault();
                 if (usr==null && password == passwordrepeat)
                 {
                    UserModel usrModel = new UserModel();
                    usrModel.Username = username;
                    usrModel.Password = password;
                    usrModel.Rank = "employee";
                    _context.UserModel.Add(usrModel);
                    await _context.SaveChangesAsync();

                     return Ok(usr);
                  }
                  return BadRequest();
                 
            }
            return NoContent();

           
        }

        /*[HttpPost]
        public async Task<ActionResult<UserModel>> PostLoginCredentials(LoginModel loginModel)
        {
          if (_context.UserModel == null)
          {
              return Problem("Entity set 'BookingSystemContext.UserModel'  is null.");
          }
            _context.UserModel.Add(userModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserModel", new { id = userModel.Id }, userModel);
        }*/

        // DELETE: api/UserModels/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, gigachad")]
        public async Task<IActionResult> DeleteUserModel(int id)
        {
            if (_context.UserModel == null)
            {
                return NotFound();
            }
            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            _context.UserModel.Remove(userModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserModelExists(int id)
        {
            return (_context.UserModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
