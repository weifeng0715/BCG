using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Controller
{
    [Route("checkout")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly WatchCatalog _catalog;

        public CheckoutController(WatchCatalog catalog)
        {
            _catalog = catalog;
        }

        [HttpPost]
        public IActionResult CalculateTotalCost([FromBody] List<string> watchIds)
        {
            decimal totalCost = 0;
            List<string> itemsPurchased = new List<string>();

            foreach (string watchId in watchIds)
            {
                if (_catalog.Watches.ContainsKey(watchId))
                {
                    var (name, price, discount) = _catalog.Watches[watchId];
                    totalCost += price;

                    if (discount.Quantity > 0)
                    {
                        int itemCount = watchIds.Count(id => id == watchId);
                        int discountGroups = itemCount / discount.Quantity;
                        totalCost -= discountGroups * discount.Discount;
                    }

                    itemsPurchased.Add(name);
                }
            }

            return Ok(new { price = totalCost, items = itemsPurchased });
        }
    }
}
