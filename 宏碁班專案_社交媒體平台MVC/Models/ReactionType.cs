﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace 宏碁班專案_社交媒體平台MVC.Models;

public partial class ReactionType
{
    public int CommentId { get; set; }

    public int UserId { get; set; }

    public int? ReactionType1 { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Comments Comment { get; set; }

    public virtual userInfo User { get; set; }
}