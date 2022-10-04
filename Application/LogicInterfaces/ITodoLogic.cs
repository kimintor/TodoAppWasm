using Domain.DTOs;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface ITodoLogic
{
    Task<Todo> CreateAsync(TodoCreationDto dto);
    Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchTodoParametersDto);

    Task UpdateAsync(TodoUpdateDto todo);

    Task DeleteAsync(int id);

    Task<Todo> GetByIdAsync(int id);
}