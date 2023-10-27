using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext dbContext = new();
            Console.WriteLine(Employee.GetEmployeesFullInformation(dbContext));
        }
    }
}