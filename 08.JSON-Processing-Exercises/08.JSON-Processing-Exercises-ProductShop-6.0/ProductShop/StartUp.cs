using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Xml.Linq;

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
            //Console.WriteLine(GetProductsInRange(context));
            //Console.WriteLine(GetSoldProducts(context));
            //Console.WriteLine(  GetCategoriesByProductsCount(context));
            Console.WriteLine(GetUsersWithProducts(context));

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
                    price = e.Price,
                    seller = string.Join(' ', e.Seller.FirstName, e.Seller.LastName)
                })
                .OrderBy(p => p.price)
                .ToArray();

            string productsJson = JsonConvert.SerializeObject(products, Formatting.Indented);
            return productsJson;
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(p => p.BuyerId != null))
                .Select(e => new
                {
                    firstName = e.FirstName,
                    lastName = e.LastName,
                    soldProducts = e.ProductsSold
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName
                    })
                    .ToArray()
                })
                .OrderBy(l => l.lastName)
                .ThenBy(f => f.firstName)
                .ToArray();

            string usersJson = JsonConvert.SerializeObject(users, Formatting.Indented);
            return usersJson;

        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(e => new
                {
                    category = e.Name,
                    productsCount = e.CategoriesProducts.Count(),
                    averagePrice = e.CategoriesProducts.Average(a => a.Product.Price).ToString("f2"),
                    totalRevenue = e.CategoriesProducts.Sum(s => s.Product.Price).ToString("f2")
                })
                .OrderByDescending(x => x.productsCount)
                .ToArray();

            string categoriesJson = JsonConvert.SerializeObject(categories, Formatting.Indented);
            return categoriesJson;
        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(x => x.BuyerId != null))
                .OrderByDescending(x => x.ProductsSold.Where(b => b.BuyerId != null).Count())
                .Select(e => new
                {
                    firstName = e.FirstName,
                    lastName = e.LastName,
                    age = e.Age,
                    products = e.ProductsSold
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price.ToString("f2")
                    })
                    .ToArray()
                })
                .ToArray();

            var output = new

            {
                usersCount = users.Count(),
                users = users
                .Select(e => new
                {
                    e.firstName,
                    e.lastName,
                    e.age,
                    soldProducts = new
                    {
                        count = e.products.Count(),
                        products = e.products
                        .Select(p => new
                        {
                            p.name,
                            p.price
                        })
                    }
                })

            };

            string usersProducts = JsonConvert.SerializeObject(output, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore

            })  ;      
            return usersProducts;
        }
    }
}