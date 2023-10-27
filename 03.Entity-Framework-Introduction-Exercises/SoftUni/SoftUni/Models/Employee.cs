﻿using SoftUni.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftUni.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string JobTitle { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int? ManagerId { get; set; }

    public DateTime HireDate { get; set; }

    public decimal Salary { get; set; }

    public int? AddressId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Employee> InverseManager { get; set; } = new List<Employee>();

    public virtual Employee? Manager { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    public virtual ICollection<EmployeeProject> EmployeesProjects { get; set; } = new List<EmployeeProject>();

    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        var result = context.Employees
            .OrderBy(e => e.EmployeeId)
            .ToArray();
        var sb = new StringBuilder();
        foreach (var employee in result) 
        {
            sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
        }

        return sb.ToString().Trim();

    }
}
