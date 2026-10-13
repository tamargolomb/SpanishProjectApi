using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Levels
{
    //ה־UpdateLevelDTO מאפשר לשנות שם ותיאור של רמה. את מזהה הרמה אפשר להעביר בכתובת הבקשה, ולכן אין חובה לכלול אותו גם ב־DTO.
    public class UpdateLevelDTO
    {
        public string LevelName { get; set; } = string.Empty;

        public string? LevelDescription { get; set; }
    }

}
