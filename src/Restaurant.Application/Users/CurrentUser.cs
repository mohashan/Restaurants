﻿namespace Restaurants.Application.Users;

public record CurrentUser(string Id,
    string Email,
    IEnumerable<string> Roles,
    string? Nationality,
    DateOnly? DateOfBirth)
{
    //public string Id { get; set; } = Id;
    //public string Email { get; set; } = Email;
    //public string? Nationality { get; set; } = Nationality;
    //public DateOnly? DateOfBirth { get; set; } = DateOfBirth;
    public bool IsInRole(string role) => Roles.Contains(role);
}