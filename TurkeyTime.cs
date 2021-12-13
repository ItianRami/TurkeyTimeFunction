using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TurkeyTimeFunction.Models;

namespace TurkeyTimeFunction
{
    public static class TurkeyTime
    {
        [Function("get_fooditems")]
        public static async Task<IActionResult> GetFoodItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fooditem")] HttpRequestData req)
        {
            //log.LogInformation("Called get_fooditems with GET request");

            return new OkObjectResult(FoodItemStore.fooditems);
        }

        [Function("get_fooditems_byID")]
        public static async Task<IActionResult> GetFoodItemsById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fooditem/{id}")] HttpRequestData req, string id)
        {
            //log.LogInformation("Called get_fooditems_byID with GET request");

            var foodItem = FoodItemStore.fooditems.FirstOrDefault(f => f.Id.Equals(id));
            if (foodItem == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(foodItem);
        }

        [Function("add_fooditem")]
        public static async Task<IActionResult> AddFoodItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "fooditem")] HttpRequestData req)
        {
            //log.LogInformation("Called add_fooditem with POST request");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var fooditem = JsonConvert.DeserializeObject<FoodItem>(requestBody);

            FoodItemStore.fooditems.Add(fooditem);

            return new OkObjectResult(fooditem);
        }

        [Function("delete_fooditem")]
        public static async Task<IActionResult> DeleteFoodItem(
           [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "fooditem/{id}")] HttpRequestData req, string id)
        {
            //log.LogInformation("Called delete_fooditem with DELETE request.");

            var fooditem = FoodItemStore.fooditems.FirstOrDefault(f => f.Id.Equals(id));

            if (fooditem == null)
            {
                return new NotFoundResult();
            }

            FoodItemStore.fooditems.Remove(fooditem);
            return new OkResult();
        }

    }
}
