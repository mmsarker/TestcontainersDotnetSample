using ProductManagement.Data;

namespace ProductManagement.Core
{
    public interface IProductService
    {
        IList<Product> GetAll();
    }
}