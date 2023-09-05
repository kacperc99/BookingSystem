namespace BookingSystem.Model
{
  public class LoggedInUser
  {
    public int UserId { get; set; }
    public LoggedInUser() { }
    public static readonly LoggedInUser Instance = new LoggedInUser();
  }
}
