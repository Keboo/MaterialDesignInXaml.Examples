using System;

namespace EFCore.DependencyInjection.Data;

public class Order
{
    public int Id { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public int OwnerId { get; set; }
    public virtual Person? Owner { get; set; }
}
