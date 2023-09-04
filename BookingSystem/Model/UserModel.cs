using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Model
{
  public class UserModel
  { 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Rank { get; set; }
    public ICollection<ReservationModel> Reservations { get; set; }
    public UserModel() { }
  }
}
