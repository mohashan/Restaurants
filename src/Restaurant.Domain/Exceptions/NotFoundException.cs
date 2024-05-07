namespace Restaurants.Domain.Exceptions;

public class NotFoundException(string resourceType, string Identifier)
    :Exception($"{resourceType} with Id : {Identifier} is not exist")
{

}

public class NotFoundException<T>(string Identifier)
    : NotFoundException(typeof(T).Name, Identifier)
{

}
