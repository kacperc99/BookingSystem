using BookingSystem.Model;

namespace BookingSystem.Service
{
  public interface IUserService
  {
    public UserModel register(string username, string password, string passwordrepeat);
  }
}
