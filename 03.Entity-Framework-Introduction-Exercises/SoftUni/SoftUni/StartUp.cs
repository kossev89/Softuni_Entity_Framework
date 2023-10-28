using SoftUni.Data;
using SoftUni.Models;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            Console.WriteLine(GetEmployeesInPeriod(context));
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString().Trim();
        }
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} - {item.Salary:f2}");
            }
            return sb.ToString().Trim();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.DepartmentId == 6)
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e=> new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department.Name,
                    e.Salary
                })
                .ToArray();
                
            var output = new StringBuilder();

            foreach (var e in employees)
            {
                output.AppendLine($"{e.FirstName} {e.LastName} from {e.Name} - ${e.Salary:f2}");
            }

            return output.ToString().Trim();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var address = new Address { AddressText = "Vitoshka 15", TownId = 4 };
            context.Add(address);
            var employee = context.Employees
                .Where(e => e.LastName == "Nakov")
                .First();
            employee.Address = address;
            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e=>e.Address.AddressText)
                .ToArray();

            var result = string.Join(Environment.NewLine, employees).ToString();
            return result;
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    employeeFirstName = e.FirstName,
                    employeeLastName = e.LastName,
                    managerFirstName = e.Manager.FirstName,
                    managerLastName = e.Manager.LastName,
                    e.EmployeeId
                })
                .Take(10)
                .ToArray();

            var projects = context.Projects
                .Select(p => new
                {
                    p.Name,
                    p.StartDate,
                    p.EndDate,
                    p.EmployeesProjects
                })
                .ToArray();

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.employeeFirstName} {e.employeeLastName} - Manager: {e.managerFirstName} {e.managerLastName}");
                var projectsOfEmployee = projects
                    .Where(x => x.EmployeesProjects.Where(y => y.EmployeeID == e.EmployeeId).Any())
                    .ToArray();
                if (projectsOfEmployee.Any(x => x.StartDate.Year >= 2001 && x.StartDate.Year <= 2003))
                {
                    foreach (var p in projectsOfEmployee.Where(x => x.StartDate.Year >= 2001 && x.StartDate.Year <= 2003))
                    {
                        if (p.EndDate != null)
                        {
                            sb.AppendLine($"--{p.Name} - {p.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - {p.EndDate?.ToString("M/d/yyyy h:mm:ss tt")}");
                        }
                        else
                        {
                            sb.AppendLine($"--{p.Name} - {p.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - not finished");
                        }
                    }
                }
            }
            return sb.ToString().Trim();     
        }
    }
}