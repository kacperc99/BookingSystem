using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Model
{
  public class ReservationModel
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime BookingBeginDate { get; set; }
    public DateTime BookignEndDate { get; set; }
    [ForeignKey("DeskModel")]
    public int DeskId { get; set; }
    public DeskModel Desk { get; set; }
    [ForeignKey("UserModel")]
    public int UserId { get; set; }
    public UserModel User { get; set; }
  }
}
