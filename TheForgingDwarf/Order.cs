using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheForgingDwarf
{
    class Order
    {
        public int Number { get; set; }
        public DateTime DateStart { get; set; }
        public int StatusCode { get; set; }
        public int ClientID { get; set; }
        public bool IsCustom { get; set; }
        public string Name { get; set; }
        public int TypeCode { get; set; }
        public int StyleCode { get; set; }
        public int SteelCode { get; set; }
        public decimal Price { get; set; }
    }
}
