namespace Restaurants.Domain.Exceptions;

public class ForbidException(string resourceType, string Identifier)
    : Exception($"{resourceType} with Id : {Identifier} is not accessible for you")
{

}


public class ForbidException<T>(string Identifier)
    : ForbidException(typeof(T).Name, Identifier)
{

}