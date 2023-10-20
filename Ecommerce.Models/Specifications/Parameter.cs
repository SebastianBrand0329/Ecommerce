using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Specifications
{
    public class Parameter
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 4;
    }
}
