using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Models.Enums
{
    public enum SaleStatus : byte
    {
        Peding = 1,
        Billed = 2,
        Canceled = 3
    }
}
