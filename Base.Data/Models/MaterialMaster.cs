using System;
using System.Collections.Generic;

namespace Base.Data.Models;

public partial class MaterialMaster
{
    public int Id { get; set; }

    public int Material { get; set; }

    public string DpName { get; set; } = null!;

    public string Description { get; set; } = null!;
}
