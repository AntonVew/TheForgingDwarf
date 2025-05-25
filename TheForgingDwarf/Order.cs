using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TheForgingDwarf
{
    public class Order
    {
        [Key]
        public int Number { get; set; }
        public DateTime DateStart { get; set; }
        public int StatusCode { get; set; }
        public int ClientID { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}