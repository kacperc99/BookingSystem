using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Model
{
  public class LocationModel
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Location_Name { get; set; }
    public ICollection<DeskModel> Desks { get; set; }
    public LocationModel() { }
  }
}
