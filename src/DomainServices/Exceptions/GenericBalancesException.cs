using System;

namespace DomainServices.Exceptions;

public class GenericBalancesException : Exception
{
	public GenericBalancesException(string errorMessage) : base(errorMessage) { }
}