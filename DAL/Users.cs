using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public int LevelId { get; set; }
        public DateTime JoinDate { get; set; }
        public int Score { get; set; }
        public string Role { get; set; } = "User";
        public Levels Level {  get; set; }
        public  List<Lesson> Lessons { get; set; } = new();
        public  List<ReviewsAndRatings> ReviewsAndRatings { get; set; }
    }
}
