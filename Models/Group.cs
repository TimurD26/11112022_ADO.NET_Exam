using System;
using System.Collections.Generic;

namespace _11112022_ADO.NET_Exam.Models;

public partial class Group
{
    public int Id { get; set; }
    public int Owner_Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
}
