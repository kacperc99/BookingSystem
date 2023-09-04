using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Model
{
  public class DeskModel
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string DeskStatus { get; set; }
    [ForeignKey("LocationModel")]
    public int LocationID { get; set; }
    public LocationModel Location { get; set; }
    public ICollection<ReservationModel> Reservations { get; set; }
    public DeskModel() { }

  }
}
