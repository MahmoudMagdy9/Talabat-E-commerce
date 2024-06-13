using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastructure.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.ProductBrands.Any())
            {
                var brandsData = await File.ReadAllTextAsync("../Talabat.Infrastructure/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                 
                if (brands != null)
                    foreach (var brand in brands)
                    {
                        dbContext.Set<ProductBrand>().Add(brand);
                    }

                await dbContext.SaveChangesAsync();
            }
            if (!dbContext.ProductCategories.Any())
            {
                var categoriesData = await File.ReadAllTextAsync("../Talabat.Infrastructure/Data/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);


                if (categories != null)
                    foreach (var category in categories)
                    {
                        dbContext.Set<ProductCategory>().Add(category);
                    }

                await dbContext.SaveChangesAsync();
            }
            if (!dbContext.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("../Talabat.Infrastructure/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);


                if (products != null)
                    foreach (var product in products)
                    {
                        dbContext.Set<Product>().Add(product);
                    }

                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.DeliveryMethods.Any())
            {
                var deliveryMethodData = await File.ReadAllTextAsync("../Talabat.Infrastructure/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodData);


                if (deliveryMethods != null)
                    foreach (var deliveryMethod in deliveryMethods)
                    {
                        dbContext.Set<DeliveryMethod>().Add(deliveryMethod);
                    }

                await dbContext.SaveChangesAsync();
            }

        }
    }
}
