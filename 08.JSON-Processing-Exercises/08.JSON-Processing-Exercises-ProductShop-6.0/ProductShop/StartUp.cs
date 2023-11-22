using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using System.Text.Json.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {

            ProductShopContext context = new ProductShopContext();
            //string usersJson = File.ReadAllText(@"..\..\..\Datasets\users.json");
            //Console.WriteLine(ImportUsers(context, usersJson));
            //string productsJson = File.ReadAllText(@"..\..\..\Datasets\products.json");
            //Console.WriteLine(ImportProducts(context, productsJson));
            //string categoriesJson = File.ReadAllText(@"..\..\..\Datasets\categories.json");
            //Console.WriteLine(ImportCategories(context, categoriesJson));
            //string categoriesProductsJson = File.ReadAllText(@"..\..\..\Datasets\categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, categoriesProductsJson));
            Console.WriteLine(  GetProductsInRange(context));

        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            User[] users = JsonConvert.DeserializeObject<User[]>(inputJson);
            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Length}";
        }
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {

            Product[] products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Length}";
        }
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(inputJson)
                .Where(x => x.Name != null)
                .ToList();

            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count}";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            CategoryProduct[] categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoriesProducts.AddRange(categoriesProducts);
            context.SaveChanges();
            return $"Successfully imported {categoriesProducts.Length}";
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(e => new
                {
                    name = e.Name,
                    price=e.Price,
                    seller = string.Join(' ', e.Seller.FirstName, e.Seller.LastName)
                })
                .OrderBy(p => p.price)
                .ToArray();

            string productsJson = JsonConvert.SerializeObject(products, Formatting.Indented);
            return productsJson;
        }
    }
}