using System;
using System.Collections.Generic;

namespace EFCore.DependencyInjection.Data;

public class Person
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }

    public virtual List<Order>? Orders { get; set; }
}
