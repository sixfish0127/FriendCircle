﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace 宏碁班專案_社交媒體平台MVC.Models;

public partial class userInfo
{
    public int id { get; set; }
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "無效的電子郵件格式")]
    public string email { get; set; }
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "密碼必須至少包含8個字符，且包含至少一個字母和一個數字")]
    public string password { get; set; }

    public string name { get; set; }

    public string userimage { get; set; }

    public string birthday { get; set; }
    [RegularExpression(@"^09\d{8}$", ErrorMessage = "無效的電話號碼")]
    public string phone { get; set; }

    public string address { get; set; }

    public string description { get; set; }

    public UserStatus status { get; set; }

    public DateTime? initDate { get; set; }
    [NotMapped]
    public string confirmPassword { get; set; }

    public virtual ICollection<CommentFavorites> CommentFavorites { get; set; } = new List<CommentFavorites>();

    public virtual ICollection<CommentShares> CommentShares { get; set; } = new List<CommentShares>();

    public virtual ICollection<Comments> Comments { get; set; } = new List<Comments>();

    public virtual ICollection<FriendShip> FriendShipUserId1Navigation { get; set; } = new List<FriendShip>();

    public virtual ICollection<FriendShip> FriendShipUserId2Navigation { get; set; } = new List<FriendShip>();

    public virtual ICollection<Notifications> NotificationsFriendRequest { get; set; } = new List<Notifications>();

    public virtual ICollection<Notifications> NotificationsUser { get; set; } = new List<Notifications>();

    public virtual ICollection<Posts> Posts { get; set; } = new List<Posts>();

    public virtual ICollection<ReactionType> ReactionType { get; set; } = new List<ReactionType>();
}