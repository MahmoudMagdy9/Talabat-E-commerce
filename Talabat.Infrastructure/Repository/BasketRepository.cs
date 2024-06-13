using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;


namespace Talabat.Infrastructure.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket) ;
        }

        public async Task<CustomerBasket?> UpdateAsync(CustomerBasket basket)
        {
            var createdOrUpdated =
                await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if (createdOrUpdated is false) return null;
            return await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }
    }
}
