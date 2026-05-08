using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();

        Task<Product> GetProductById(int product_id);

        Task<List<Product>> GetProductsByName(string query);
    }
}
