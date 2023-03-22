using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : Controller
    {

        private readonly ProductService productService;

        public ProductController()
        {
            productService = new();
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(productService.GetProducts());
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(string id)
        {
            return Ok(productService.GetProduct(id));
        }
    }
}
