using Domain.DTOs;

namespace Application.LogicInterfaces;
using Domain.Models;

public interface IUserLogic
{
    Task<User> CreateAsync(UserCreationDto userToCreate);
}