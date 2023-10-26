﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;

namespace SoftUni.Models;

public partial class Employee
{
    [Key]
    [Column("EmployeeID")]
    public int EmployeeId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? MiddleName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string JobTitle { get; set; } = null!;

    [Column("DepartmentID")]
    public int DepartmentId { get; set; }

    [Column("ManagerID")]
    public int? ManagerId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime HireDate { get; set; }

    [Column(TypeName = "decimal(15, 4)")]
    public decimal Salary { get; set; }

    [Column("AddressID")]
    public int? AddressId { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Employees")]
    public virtual Address? Address { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("Employees")]
    public virtual Department Department { get; set; } = null!;

    [InverseProperty("Manager")]
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    [InverseProperty("Manager")]
    public virtual ICollection<Employee> InverseManager { get; set; } = new List<Employee>();

    [ForeignKey("ManagerId")]
    [InverseProperty("InverseManager")]
    public virtual Employee? Manager { get; set; }

  
    public virtual ICollection<EmployeeProject> EmployeesProjects { get; set; } = new List<EmployeeProject>();

    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        var employees = context.Employees
            .Where(e => e.Salary > 50000)
            .OrderBy(e => e.FirstName)
            .ToArray();

        var sb = new StringBuilder();

        foreach (var item in employees)
        {
            sb.AppendLine($"{item.FirstName} – {item.Salary:f2}");
        }
        return sb.ToString().Trim();
    }
}
