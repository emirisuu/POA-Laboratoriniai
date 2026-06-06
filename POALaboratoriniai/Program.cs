using Microsoft.EntityFrameworkCore;
using POALaboratoriniai;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/api/health", () => "API is running and healthy" )
    .WithName("APIHealth");

app.MapGet("/api/hello", () => "Hello World!" )
    .WithName("HelloWorld");

app.MapGet("/api/students", async (AppDbContext dbContext) => await dbContext.Students.ToListAsync())
    .WithName("Students");

app.MapPost("/api/students", async (Student student, AppDbContext dbContext) => 
{
    await dbContext.Students.AddAsync(student);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/api/students/{student.Id}", student);
})
    .WithName("CreateStudent");

app.Run();