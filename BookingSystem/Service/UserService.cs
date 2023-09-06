using BookingSystem.Data;
using BookingSystem.Model;

namespace BookingSystem.Service
{
  public class UserService : IUserService
  {
    private readonly BookingSystemContext _context;

    public UserService(BookingSystemContext context)
    {
      _context = context;
    }

    public UserModel register (string username, string password, string passwordrepeat)
    {
      if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(passwordrepeat))
      {
        var usr = _context.UserModel.Where(e => e.Username == username).FirstOrDefault();
        if (usr == null && password == passwordrepeat)
        {
          UserModel usrModel = new UserModel();
          usrModel.Username = username;
          usrModel.Password = password;
          usrModel.Rank = "employee";
          _context.UserModel.Add(usrModel);
          _context.SaveChangesAsync();

          return usrModel;
        }
        return null;

      }
      return null;
    }

    private UserModel BadRequest()
    {
      throw new NotImplementedException();
    }
  }
}
