using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using TestAPIs.Models;
using TestAPIs.Services;

namespace TestAPIs.Controllers
{
    [Route(Constants.BaseAddress)]
    [ApiController]
    public class WooliesXController : Controller
    {
        private readonly ILogger<WooliesXController> _logger;
        private readonly ISettingService _settingService;
        private readonly IProductService _productService;
        private readonly ITrolleyService _trolleyService;
        public WooliesXController(ISettingService settingService, IProductService productService, ITrolleyService trolleyService, ILogger<WooliesXController> logger)
        {
            _settingService = settingService;
            _productService = productService;
            _trolleyService = trolleyService;
            _logger = logger;
        }

        /// <summary>
        /// This will call your api looking for a resource available at your base url /user. For example, if the url you request is "http://localhost:5001/api/answers" this will make a GET request to "http://localhost:5001/api/answers/user" The result will be a JSON object in the format {"name": "test", "token" : "1234-455662-22233333-3333"}
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(User))]
        public IActionResult Get()
        {
            try
            {
                var user = _settingService.WooliesXUser;
                return Ok(user);
            }
            catch (Exception ex)
            {
                var errorMessage = "Failed to return User.";
                _logger.LogError(ex, errorMessage);
                return BadRequest(errorMessage);
            }
        }

        /// <summary>
        /// This will call your api looking for a range of different sorting options at your base url /sort Your Api needs to call the "products" resource to get a list of available products This endpoint will need to accept a query string parameter called "sortOption" which will take in the following strings - "Low" - Low to High Price - "High" - High to Low Price - "Ascending" - A - Z sort on the Name - "Descending" - Z - A sort on the Name - "Recommended" - this will call the "shopperHistory" resource to get a list of customers orders and needs to return based on popularity, Your response will be in the same data structure as the "products" response (only sorted correctly)
        /// </summary>
        /// <param name="sortOption"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("sort")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IList<Product>))]
        public async Task<IActionResult> GetSortedProducts([FromQuery] string sortOption)
        {
            var products = await _productService.GetProductsAsync(sortOption);

            if (products == null)
            {
                return BadRequest("Failed to return sorted product list.");
            }

            return Ok(products);
        }

        [HttpPost]
        [Route("trolleyTotal")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(decimal))]
        public async Task<IActionResult> GetTrolleyTotal([FromBody, Required] TrolleyItems trolleyItems)
        {
            try
            {
                var totalPrice = await _trolleyService.GetTrolleyTotalX(trolleyItems);
                return Ok(totalPrice);
            }
            catch (Exception ex)
            {
                var errorMessage = "Failed to calculate total price for trolley items.";
                _logger.LogError(ex, errorMessage);
                return BadRequest(errorMessage);
            }
        }
    }
}
