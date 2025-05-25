using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TheForgingDwarf
{
    public class Product
    {
        public enum ProdType
        {
            Weapon = 0,
            Gates = 1,
            Sculpture = 2,
            MedievalArmor = 3,
            RenaissanceArmor = 4
        }

        public enum ProdStyle
        {
            Gothic = 1,
            Fantasy = 2,
            Historical = 3
        }

        public enum ProdSteel
        {
            Damascus = 1,
            Mosaic = 2,
            Crucible = 3
        }

        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public bool IsCustom { get; set; }
        public ProdType Type { get; set; }
        public ProdStyle Style { get; set; }
        public ProdSteel Steel { get; set; }
        public decimal Price { get; set; }
    }
}
