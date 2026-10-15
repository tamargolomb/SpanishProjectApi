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
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public int LevelId { get; set; }
        public DateTime JoinDate { get; set; }
        public int Score { get; set; }
        public  List<Levels> Levels { get; set; }
        public  List<ReviewsAndRatings> ReviewsAndRatings { get; set; }
    }
}
