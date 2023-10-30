﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class CompanyVM
    {
        public Company Company { get; set; }

        public IEnumerable<SelectListItem> listWarehouse { get; set; }
    }
}
