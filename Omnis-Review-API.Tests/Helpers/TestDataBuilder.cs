using System;
using OmnisReview.Models;

namespace OmnisReview.Tests.Helpers;

public class UserBuilder
{
    private ApplicationUser _user;

    public UserBuilder()
    {
        _user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            UserName = "testuser",
            Name = "Test User",
            Birth_Date = new DateTime(1990, 1, 1),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
    }

    public UserBuilder WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    public UserBuilder WithUserName(string userName)
    {
        _user.UserName = userName;
        return this;
    }

    public UserBuilder WithName(string name)
    {
        _user.Name = name;
        return this;
    }

    public UserBuilder WithBirthDate(DateTime birthDate)
    {
        _user.Birth_Date = birthDate;
        return this;
    }

    public ApplicationUser Build() => _user;
}

public class LoginDtoBuilder
{
    private LoginDto _loginDto;

    public LoginDtoBuilder()
    {
        _loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "Password123!"
        };
    }

    public LoginDtoBuilder WithEmail(string email)
    {
        _loginDto.Email = email;
        return this;
    }

    public LoginDtoBuilder WithPassword(string password)
    {
        _loginDto.Password = password;
        return this;
    }

    public LoginDto Build() => _loginDto;
}

public class RegisterDtoBuilder
{
    private RegisterDto _registerDto;

    public RegisterDtoBuilder()
    {
        _registerDto = new RegisterDto
        {
            Name = "Test User",
            Email = "test@example.com",
            UserName = "testuser",
            Birth_Date = new DateTime(1990, 1, 1),
            Password = "Password123!"
        };
    }

    public RegisterDtoBuilder WithEmail(string email)
    {
        _registerDto.Email = email;
        return this;
    }

    public RegisterDtoBuilder WithUserName(string userName)
    {
        _registerDto.UserName = userName;
        return this;
    }

    public RegisterDtoBuilder WithName(string name)
    {
        _registerDto.Name = name;
        return this;
    }

    public RegisterDtoBuilder WithPassword(string password)
    {
        _registerDto.Password = password;
        return this;
    }

    public RegisterDto Build() => _registerDto;
}
