using System.Collections.Generic;
using System.Threading.Tasks;
using GrainInterfaces.States;
using Orleans;

namespace GrainInterfaces
{
    public interface ICartGrain : IGrainWithGuidKey
    {
      Task<Cart> GetCart();
      Task<List<Product>> GetProducts();
      Task AddProduct(Product product);
    }
}
