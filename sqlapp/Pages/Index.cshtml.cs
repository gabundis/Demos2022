using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sqlapp.Models;
using sqlapp.Services;

namespace sqlapp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        public List<Product> Products;
        public bool IsBeta;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public void OnGet()
        {
            IsBeta = _productService.IsBeta().Result;
            //ProductService productService = new ProductService();
            Products = _productService.GetProducts();
            //Products = _productService.GetProducts().GetAwaiter().GetResult();
        }
    }
}