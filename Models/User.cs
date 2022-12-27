using System;
using System.Collections.Generic;

namespace _11112022_ADO.NET_Exam.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string? Password { get; set; }

    public virtual ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
}
