using System;

namespace DomainModels;
public abstract class BaseModel
{
    public Guid Id { get; set; }
    public readonly DateTime CreatedAt = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; } = null;
}