using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrainInterfaces;
using GrainInterfaces.States;
using Orleans;
using Orleans.Providers;

namespace Grains
{
    [StorageProvider(ProviderName = "CartStorage")]
    public class CartGrain : Grain<CartState>, ICartGrain
    {
        public async Task AddProduct(Product product)
        {
            await ReadStateAsync();

            if (State.State == null)
                State = new CartState();

            State.State.Products.Add(product);
            await WriteStateAsync();
        }

        public async Task<Cart> GetCart()
        {

            await ReadStateAsync();

            if (State.State != null)
                return State.State;

            State.State = new Cart
            {
                Id = Guid.NewGuid(),
                Products = new List<Product>()
            };

            await WriteStateAsync();

            return State.State;
        }

        public async Task<List<Product>> GetProducts()
        {
            await ReadStateAsync();
            return State.State.Products;
        }
    }
}
