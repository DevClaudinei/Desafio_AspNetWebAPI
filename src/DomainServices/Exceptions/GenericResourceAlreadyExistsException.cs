using System;

namespace DomainServices.Exceptions;

public class GenericResourceAlreadyExistsException : Exception
{
	public GenericResourceAlreadyExistsException(string errorMessage) : base(errorMessage) { }

}