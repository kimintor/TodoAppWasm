using Domain.DTOs;

namespace Application.LogicInterfaces;
using Domain.Models;

public interface IUserLogic
{
     Task<User> CreateAsync(UserCreationDto userToCreate);
     Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters);
}