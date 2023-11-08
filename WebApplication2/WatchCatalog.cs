using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class WatchCatalog
    {
        public Dictionary<string, (string Name, decimal Price, (int Quantity, decimal Discount) Discount)> Watches { get; } = new()
        {
            ["001"] = ("Rolex", 100, (3, 300)),
            ["002"] = ("Michael Kors", 80, (2, 120)),
            ["003"] = ("Swatch", 50, (1, 0)),
            ["004"] = ("Casio", 30, (1, 0))
        };
    }
}
