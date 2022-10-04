using Domain.DTOs;
using Domain.Models;

namespace Application.DaoInterfaces;

public interface ITodoDao
{
    Task<Todo> CreateAsync(Todo todo);
    Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchTodoParametersDto);

    Task UpdateAsync(Todo todo);

    Task<Todo> GetByIdAsync(int id);

    Task DeleteAsync(int id);
}