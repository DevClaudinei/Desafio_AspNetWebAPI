using System;

namespace DomainServices.Exceptions;

public class CustomerException : Exception
{
	public CustomerException(string errorMessage) : base(errorMessage) { }
}