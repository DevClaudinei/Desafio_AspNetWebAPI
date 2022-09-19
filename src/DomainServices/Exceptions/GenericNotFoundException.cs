using System;

namespace DomainServices.Exceptions;

public class GenericNotFoundException : Exception
{
    public GenericNotFoundException(string errorMessage) : base(errorMessage) { }
}