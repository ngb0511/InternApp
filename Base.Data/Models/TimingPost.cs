using System;
using System.Collections.Generic;

namespace Base.Data.Models;

public partial class TimingPost
{
    public int Id { get; set; }

    public string Customer { get; set; } = null!;

    public string PostName { get; set; } = null!;

    public DateTime PostStart { get; set; }

    public DateTime PostEnd { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public int CreatedBy { get; set; } = 1;

    public virtual UserAssign CreatedByNavigation { get; set; } = null!;
}
