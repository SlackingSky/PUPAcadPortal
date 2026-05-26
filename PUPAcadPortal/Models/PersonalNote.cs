using System;
using System.Collections.Generic;

namespace PUPAcadPortal.Models;

public partial class PersonalNote
{
    public int NoteId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
