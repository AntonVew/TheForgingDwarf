using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgingDwarf
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public bool IsSuperUser { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
