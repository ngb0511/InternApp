using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Data.Models;

public partial class TimingPost
{
    public int Id { get; set; }

    public string Customer { get; set; } = null!;

    public string PostName { get; set; } = null!;
    [NotMapped]
    public DateOnly PostStart { get; set; }
    [NotMapped]
    public DateOnly PostEnd { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public virtual UserAssign CreatedByNavigation { get; set; } = null!;
}
