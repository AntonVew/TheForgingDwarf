using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgingDwarf
{
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RaceCode { get; set; }

        public bool IsSuperUser { get; set; }
    }
}
