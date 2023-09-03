using Microsoft.AspNetCore.Mvc;
using ProductManagement.Core;
using ProductManagement.Data;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        [Route("products")]
        public IEnumerable<Product> GetProducts()
        {
            return this.productService.GetAll();            
        }        
    }
}
