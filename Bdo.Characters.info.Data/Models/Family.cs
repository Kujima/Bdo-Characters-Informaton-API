using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bdo.Characters.info.Data.Models
{
    public class Family
    {
        public string Name { get; set; }
        public List<Character> Characters { get; set; }
    }
}
