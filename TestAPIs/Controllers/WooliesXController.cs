using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [HttpGet]
        [Route("user")]
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

        [HttpGet]
        [Route("sort")]
        public async Task<IActionResult> GetSortedProducts([FromQuery, Required] string sortOption)
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
