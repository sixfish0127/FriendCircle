﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace 宏碁班專案_社交媒體平台MVC.Models;

public partial class Posts
{
    public int Id { get; set; }    

    public int UserId { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Comments> Comments { get; set; } = new List<Comments>();

    public virtual userInfo User { get; set; }
}