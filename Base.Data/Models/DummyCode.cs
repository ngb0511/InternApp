using System;
using System.Collections.Generic;

namespace Base.Data.Models;

public partial class DummyCode
{
    public int Id { get; set; }

    public int Material { get; set; }

    public string DpName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int TotalMapping { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public virtual UserAssign CreatedByNavigation { get; set; } = null!;
}
