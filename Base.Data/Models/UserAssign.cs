using Base.Data.Models;
using System;
using System.Collections.Generic;

namespace Base.Data.Models;

public partial class UserAssign
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string UserFullName { get; set; } = null!;

    public virtual ICollection<DummyCode> DummyCodes { get; set; } = new List<DummyCode>();

    public virtual ICollection<Forecast> Forecasts { get; set; } = new List<Forecast>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual ICollection<TimingPost> TimingPosts { get; set; } = new List<TimingPost>();
}
