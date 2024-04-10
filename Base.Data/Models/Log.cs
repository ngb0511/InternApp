using System;
using System.Collections.Generic;

namespace Base.Data.Models;

public partial class Log
{
    public int Id { get; set; }

    public string Detail { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public virtual UserAssign CreatedByNavigation { get; set; } = null!;
}
