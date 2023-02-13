using Microsoft.EntityFrameworkCore;
using westcoasteducation.api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<WestCoastEducationContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Seed database
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<WestCoastEducationContext>();
    await context.Database.MigrateAsync();
    await SeedData.LoadTeacherData(context);
    await SeedData.LoadCourseData(context);
    await SeedData.LoadStudentData(context);
    await SeedData.LoadQualificationData(context);
}
catch (Exception ex)
{
    Console.WriteLine("{0}", ex.Message);
    throw;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
