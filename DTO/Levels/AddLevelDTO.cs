using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Levels
{

    public class AddLevelDTO
    {
        public string LevelName { get; set; } = string.Empty;

        public string? LevelDescription { get; set; }
    }

}
