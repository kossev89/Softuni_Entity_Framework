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
            string usersJson = File.ReadAllText(@"..\..\..\Datasets\users.json");
            Console.WriteLine(ImportUsers(context, usersJson));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            User[] users = JsonConvert.DeserializeObject<User[]>(inputJson);
            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Length}";
        }
    }
}