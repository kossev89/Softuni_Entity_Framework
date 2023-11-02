using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            using StudentSystemContext context = new();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}