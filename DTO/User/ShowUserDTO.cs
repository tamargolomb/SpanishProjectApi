using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.User
{

    public class ShowUserDTO
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;

        public int LevelId { get; set; }

        public string? LevelName { get; set; }

        public DateTime JoinDate { get; set; }

        public int Score { get; set; }
    }

}
