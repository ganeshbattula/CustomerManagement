using CustomerManagement.API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("CustomerDB"));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    SeedData(context);
}

static void SeedData(AppDbContext context)
{
    context.Database.EnsureCreated();

    if (!context.Customers.Any())
    {
        context.Customers.AddRange(
            new Customer { Name = "Ganesh", Email = "ganesh@example.com", Phone = "123-456-7890" },
            new Customer { Name = "Harshini", Email = "harshini@example.com", Phone = "987-654-3210" }
        );

        context.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run("http://localhost:5251");
