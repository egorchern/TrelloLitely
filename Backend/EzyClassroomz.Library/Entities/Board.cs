using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EzyClassroomz.Library.Types;

namespace EzyClassroomz.Library.Entities;

public class Board
{
    [Key]
    public long Id { get; set; }
    [Required]
    [MaxLength(256)]
    public required string Name { get; set; }
    [Required]
    public required string TenantId { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}