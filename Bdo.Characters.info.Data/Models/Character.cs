using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bdo.Characters.info.Data.Models
{
    public class Character
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public List<Profession> Professions { get; set; }
    }
}
