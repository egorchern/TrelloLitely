using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EzyClassroomz.Library.Types;

namespace EzyClassroomz.Library.Entities;

public class Ticket
{
    [Key]
    public long Id { get; set; }
    [Required]
    [MaxLength(512)]
    public required string Title { get; set; }
    [Required]
    [MaxLength(50000)]
    public required string Content { get; set; }
    public long BoardId { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }
    public TicketStatus Status { get; set; } = TicketStatus.ToDo;
    public Board? Board { get; set; }

    public Ticket()
    {
    }
}