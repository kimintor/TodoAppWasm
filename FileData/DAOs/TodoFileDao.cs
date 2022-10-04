using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace FileData.DAOs;

public class TodoFileDao:ITodoDao
{

    private readonly FileContext context;

    public TodoFileDao(FileContext context)
    {
        this.context = context;
    }

    public Task<Todo> CreateAsync(Todo todo)
    {
        int id = 1;
        if (context.Todos.Any())
        {
            id = context.Todos.Max(t => t.Id);
            id++;
        }

        todo.Id = id;
        
        context.Todos.Add(todo);
        context.SaveChanges();

        return Task.FromResult(todo);
    }

    public Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchTodoParametersDto)
    {
        IEnumerable<Todo> result = context.Todos.AsEnumerable();

        if (!string.IsNullOrEmpty(searchTodoParametersDto.Username))
        {
            result = context.Todos.Where(todo =>
                todo.Owner.UserName.Equals(searchTodoParametersDto.Username, StringComparison.OrdinalIgnoreCase));
        }

        if (searchTodoParametersDto.UserId!=null)
        {
            result = result.Where(t => t.Owner.Id == searchTodoParametersDto.UserId);
        }

        if (searchTodoParametersDto.CompletedStatus!=null)
        {
            result = result.Where(t => t.IsCompleted == searchTodoParametersDto.CompletedStatus);
        }

        if (!string.IsNullOrEmpty(searchTodoParametersDto.TitleContains))
        {
            result = result.Where(t =>
                t.Title.Contains(searchTodoParametersDto.TitleContains, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(result);
    }

    public Task UpdateAsync(Todo todo)
    {
        Todo? toDelete = context.Todos.FirstOrDefault(t => t.Id == todo.Id);
        if (toDelete == null)
        {
            throw new Exception($"Could not find with id {todo.Id} todo to update");
        }

        context.Todos.Remove(toDelete);
        context.Todos.Add(todo);
        context.SaveChanges();
        
        return Task.CompletedTask;
    }

    public Task<Todo> GetByIdAsync(int id)
    {
        Todo? existing = context.Todos.FirstOrDefault(t => t.Id == id);
        if (existing== null)
        {
            throw new Exception($"Todo with id {id} does not exist");
        }

        return Task.FromResult(existing);
    }

    public Task DeleteAsync(int id)
    {
        Todo? toDelete = context.Todos.FirstOrDefault(t => t.Id == id);

        if (toDelete== null)
        {
            throw new Exception($"Todo with id {id} does not exist");
        }
        context.Todos.Remove(toDelete);
        context.SaveChanges();
        return Task.CompletedTask;
    }
}