using System;
using System.Collections.Generic;

namespace _11112022_ADO.NET_Exam.Models;

public partial class PrivateMessage
{
    public int Id { get; set; }

    public int FromUserId { get; set; }

    public int ToUserId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public bool IsUserInBlackList { get; set; }
    public bool Is_deleted { get; set; }

    public string? AdditionalInfo { get; set; }
}
