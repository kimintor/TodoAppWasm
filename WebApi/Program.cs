using Application.DaoInterfaces;
using Application.Logic;
using Application.LogicInterfaces;
using FileData;
using FileData.DAOs;

var builder = WebApplication.CreateBuilder(args);


// added by me 
builder.Services.AddScoped<FileContext>();
builder.Services.AddScoped<IUserDao, UserFileDao>();
builder.Services.AddScoped<IUserLogic, UserLogic>();
//todos controller
builder.Services.AddScoped<ITodoDao, TodoFileDao>();
builder.Services.AddScoped<ITodoLogic, TodoLogic>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();