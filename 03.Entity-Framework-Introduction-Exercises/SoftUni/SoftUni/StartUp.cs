using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;
using SoftUni.Data;
using SoftUni.Models;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            Console.WriteLine(DeleteProjectById(context));
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
                .Select(e => new
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
                .Select(e => e.Address.AddressText)
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
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .Select(e => new
                {
                    e.AddressText,
                    e.Town.Name,
                    e.Employees.Count
                })
                .OrderByDescending(e => e.Count)
                .ThenBy(e => e.Name)
                .ThenBy(e => e.AddressText)
                .Take(10)
                .ToArray();

            var output = new StringBuilder();

            foreach (var a in addresses)
            {
                output.AppendLine($"{a.AddressText}, {a.Name} - {a.Count} employees");
            }

            return output.ToString().Trim();
        }
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee147 = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.EmployeeId,
                })
                .Where(x => x.EmployeeId == 147)
                .FirstOrDefault();

            var sb = new StringBuilder();
            sb.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");

            var projectsOfEmployee = context.EmployeesProjects
                .Select(e => new
                {
                    e.Project.Name,
                    e.EmployeeID
                })
                .Where(x => x.EmployeeID == employee147.EmployeeId)
                .OrderBy(x => x.Name)
                .ToArray();

            foreach (var p in projectsOfEmployee)
            {
                sb.AppendLine(p.Name);
            }

            return sb.ToString().Trim();
        }
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Select(e => new
                {
                    e.Name,
                    e.Manager.FirstName,
                    e.Manager.LastName,
                    e.Employees.Count,
                    e.Employees
                })
                .Where(c => c.Count > 5)
                .OrderBy(c => c.Count)
                .ThenBy(d => d.Name)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.Name} – {d.FirstName} {d.LastName}");

                var employees = d.Employees
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    })
                    .OrderBy(f => f.FirstName)
                    .ThenBy(l => l.LastName)
                    .ToArray();

                foreach (var e in employees)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }
            return sb.ToString().Trim();
        }
        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .Select(e => new
                {
                    e.Name,
                    e.Description,
                    e.StartDate
                })
                .OrderByDescending(s => s.StartDate)
                .Take(10)
                .ToArray();

            var projectsSorted = projects
                .OrderBy(n => n.Name)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var p in projectsSorted)
            {
                var formatedDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                sb.AppendLine(p.Name.ToString());
                sb.AppendLine(p.Description.ToString());
                sb.AppendLine(formatedDate);
            }

            return sb.ToString().Trim();
        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var empl = context.Employees
                .Where(e => e.Department.Name == "Engineering"
                || e.Department.Name == "Tool Design"
                || e.Department.Name == "Marketing"
                || e.Department.Name == "Information Services")
                .OrderBy(f => f.FirstName)
                .ThenBy(l => l.LastName)
                .ToList();

            foreach (var e in empl)
            {
                e.Salary *= 1.12m;
            }

            context.SaveChanges();

            string result = string.Join(Environment.NewLine, empl.Select(e => $"{e.FirstName} {e.LastName} (${e.Salary:f2})"));
            return result;

        }
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            const string StartWith = "Sa";
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .Where(n => n.FirstName.StartsWith(StartWith))
                .OrderBy(f => f.FirstName)
                .ThenBy(l => l.LastName)
                .ToList();

            string result = string.Join(Environment.NewLine, employees.Select(e => $"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})"));
            return result;
        }
        public static string DeleteProjectById(SoftUniContext context)
        {
            const int IdToBeDeleted = 2;
            var projects = context.Projects.Find(IdToBeDeleted);

            var employeesWithProjects = context.EmployeesProjects
                .Where(e => e.ProjectID == IdToBeDeleted)
                .ToList();

            foreach (var e in employeesWithProjects)
            {
                context.EmployeesProjects.Remove(e);
            }

            context.Projects.Remove(projects);
            context.SaveChanges();

            var projectsLeft = context.Projects
                .Select(e => new
                {
                    e.Name
                })
                .Take(10)
                .ToList();

            string result = string.Join(Environment.NewLine, projectsLeft.Select(e => $"{e.Name}"));
            return result;
        }
    }
}