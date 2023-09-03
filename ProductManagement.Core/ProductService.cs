using ProductManagement.Data;
using ProductManagement.DataAccess;

namespace ProductManagement.Core
{
    public class ProductService:IProductService
    {
        private readonly ProductDbContext productDbContext;

        public ProductService(ProductDbContext productDbContext)
        {
            this.productDbContext = productDbContext;
        }

        public IList<Product> GetAll()
        {
            return this.productDbContext.Products.ToList();
        }
    }
}