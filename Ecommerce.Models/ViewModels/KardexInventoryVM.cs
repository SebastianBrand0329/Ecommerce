using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class KardexInventoryVM
    {
        public Product Product { get; set; }

        public IEnumerable<KardexInventory> ListKardexInventory { get; set; }

        public DateTime DateInitial { get; set; }

        public DateTime DateEnd { get; set; }
}
}
