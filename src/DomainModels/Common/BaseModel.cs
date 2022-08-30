using System;

namespace DomainModels;

public abstract class BaseModel
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}