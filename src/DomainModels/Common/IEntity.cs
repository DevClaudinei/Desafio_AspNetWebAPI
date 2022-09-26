using System;

namespace DomainModels;

public interface IEntity
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}