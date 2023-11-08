using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Controller
{
    [Route("checkout")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ILogger<CheckoutController> _logger;
        public CheckoutController(ILogger<CheckoutController> logger)
        {
            _logger = logger;
        }
        private readonly List<Watch> watchCatalog = new List<Watch>
        {
            new Watch { WatchID = "001", Name = "Rolex", UnitPrice = 100, DiscountQuantity = 3, DiscountPrice = 200 },
            new Watch { WatchID = "002", Name = "Michael Kors", UnitPrice = 80, DiscountQuantity = 2, DiscountPrice = 120 },
            new Watch { WatchID = "003", Name = "Swatch", UnitPrice = 50, DiscountQuantity=0 },
            new Watch { WatchID = "004", Name = "Casio", UnitPrice = 30, DiscountQuantity = 0}

            
        };

        [HttpPost]
        public ActionResult<decimal> CalculateTotalCost(List<string> watchIds)
        {
            if (watchIds == null || watchIds.Count == 0)
            {
                // Handle the case where the request does not contain any watch IDs.
                // You can return an error response or throw an exception, as appropriate.
                return BadRequest("No watch IDs provided.");
            }
            decimal totalCost = 0;
            // Create a dictionary to count the number of watches for each unique ID.
            Dictionary<string, int> watchCounts = new Dictionary<string, int>();

            // Count the watches for each unique ID.
            foreach (var watchId in watchIds)
            {
                if (watchCounts.ContainsKey(watchId))
                {
                    watchCounts[watchId]++;
                }
                else
                {
                    watchCounts[watchId] = 1;
                }
            }

            foreach (var watchId in watchCounts.Keys)
            {
                var watch = watchCatalog.FirstOrDefault(w => w.WatchID == watchId);
                if (watch != null)
                {
                    int quantity = watchCounts[watchId];
                    totalCost += CalculateWatchCost(watch, quantity);
                    _logger.LogInformation($"Watch: {watch.Name}, Quantity: {quantity}, Price: {CalculateWatchCost(watch, quantity)}, Total: {totalCost}");
                }
                else
                {
                    // Handle the case where a watch with the given ID is not found in the catalog.
                    // You can return an error response or throw an exception, as appropriate.
                    return NotFound($"Watch with ID {watchId} not found.");
                }
            }
            // Create the response object with headers and body
            var response = new
            {
                price = totalCost
            };

            return Ok(response);
        }

        private decimal CalculateWatchCost(Watch watch, int quantity)
        {
            if (watch.DiscountQuantity > 0 && quantity >= watch.DiscountQuantity)
            {
                int fullPriceItems = quantity / watch.DiscountQuantity;
                int remainingItems = quantity % watch.DiscountQuantity;
                _logger.LogInformation($"Watch: {watch.Name}, Discount No.: {fullPriceItems}, Remaining No.: {remainingItems}");

                return (fullPriceItems * watch.DiscountPrice) + (remainingItems * watch.UnitPrice);
            }
            else
            {
                return quantity * watch.UnitPrice;
            }
        }
    }
}
