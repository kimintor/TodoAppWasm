namespace Domain.DTOs;

public class TodoCreationDto
{
    public int OwnerId { get; set; }
    public string Title { get; set; }

    public TodoCreationDto(int ownerId, string title)
    {
        OwnerId = ownerId;
        Title = title;
    }
}