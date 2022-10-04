using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace Application.Logic;

public class TodoLogic:ITodoLogic
{
    private readonly ITodoDao todoDao;
    private readonly IUserDao userDao;

    public TodoLogic(ITodoDao todoDao, IUserDao userDao)
    {
        this.todoDao = todoDao;
        this.userDao = userDao;
    }

    public async Task<Todo> CreateAsync(TodoCreationDto dto)
    {
        User? user = await userDao.GetByIdAsync(dto.OwnerId);
        if (user==null)
        {
            throw new Exception($"User with id:{dto.OwnerId} was not found");
        }

        ValidateTodo(dto);
        Todo todo = new Todo(user, dto.Title);
        Todo created = await todoDao.CreateAsync(todo);
        return created;
    }

    public Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchTodoParametersDto)
    {
       
        return todoDao.GetAsync(searchTodoParametersDto);
    }

    public async Task UpdateAsync(TodoUpdateDto todo)
    {
        Todo? exisiting = await todoDao.GetByIdAsync(todo.Id);

        if (exisiting==null)
        {
            throw new Exception($"Todo with ID {todo.Id} not found!");
        }

        User? user = null;

        if (todo.OwnerId != null)
        {
            user = await userDao.GetByIdAsync((int)todo.OwnerId);
            if (user == null)
            {
                throw new Exception($"User with id {todo.OwnerId} was not found");
            }
        }

        if (todo.IsComplete != null && exisiting.IsCompleted && !(bool)todo.IsComplete)
        {
            throw new Exception("cannot un-complete a complete todo");
        }

        User userToUse = user ?? exisiting.Owner;
        string titleToUse = todo.Title ?? exisiting.Title;
        bool completedToUse = todo.IsComplete ?? exisiting.IsCompleted;

        Todo updated = new Todo(userToUse, titleToUse)
        {
            IsCompleted = completedToUse,
            Id = exisiting.Id,
        };
        
        ValidateTodo(updated);

        await todoDao.UpdateAsync(updated);


    }

    public async Task DeleteAsync(int id)
    {
        Todo? toDelete = await todoDao.GetByIdAsync((int) id);
        if (toDelete == null)
        {
            throw new Exception($"Todo with id {id} was not found ");
        }

        if (!toDelete.IsCompleted)
        {
            throw new Exception($"todo with id {id} cannot be deleted because is not completed");
        }

        await todoDao.DeleteAsync(id);
    }

    public Task<Todo> GetByIdAsync(int id)
    {
      return todoDao.GetByIdAsync((int)id);
    }

    private void ValidateTodo(TodoCreationDto dto)
    {
        if (string.IsNullOrEmpty(dto.Title))
        {
            throw new Exception("Title cannot be empty");
        }
    }

    private void ValidateTodo(Todo todo)
    {
        if (string.IsNullOrEmpty(todo.Title)) throw new Exception("Title cannot be empty.");
    }
}