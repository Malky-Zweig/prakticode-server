// var builder = WebApplication.CreateBuilder(args);
// var app = builder.Build();

// // שליפת כל המוצרים
// app.MapGet("/getitems", async(ToDoDbContext db) => {
//     return await db.items.ToListAsync();
// });
// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql")));
// // הוספת משימה חדשה
// app.MapPost("/tasks", (string task) => {
//     tasks.Add(task);
//     return Results.Created($"/tasks/{task}", task);
// });

// // עדכון משימה
// app.MapPut("/tasks/{index}", (int index, string task) => {
//     if (index < 0 || index >= tasks.Count)
//     {
//         return Results.NotFound();
//     }
//     tasks[index] = task;
//     return Results.NoContent();
// });

// // מחיקת משימה
// app.MapDelete("/tasks/{index}", (int index) => {
//     if (index < 0 || index >= tasks.Count)
//     {
//         return Results.NotFound();
//     }
//     tasks.RemoveAt(index);
//     return Results.NoContent();
// // });

// app.MapGet("/", () => "Hello World!");

// app.Run();
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TodoApi; // ודא שזה המרחב השמות הנכון
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// הוספת DbContext
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql"))); // השתמש ב-MySQL

// הוספת שירות CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// הוספת Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });
});

var app = builder.Build();

// הפעלת מדיניות CORS
app.UseCors("AllowAllOrigins");

// הפעלת Swagger
app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
    c.RoutePrefix = string.Empty; // אם תרצה שה-Swagger UI יהיה בדף הראשי
});

// Route לשליפת כל המשימות
app.MapGet("/tasks", async (ToDoDbContext db) =>
{
    return await db.item.ToListAsync();
})
.WithName("GetAllTasks") // הוספת שם למסלול
.Produces<List<item>>(StatusCodes.Status200OK); // הוספת תוצאה אפשרית

// Route להוספת משימה
app.MapPost("/tasks/{name}", async (ToDoDbContext db, string name) =>
{
    
   item item1=new item();
   item1.Name=name;
   item1.IsComplete=false;

    // db.item.Add(item);
    // consol.log(item)
    await db.item.AddAsync(item1);
    return await db.SaveChangesAsync();
    // return Results.Created($"/tasks/{item.Id}", item);
});
// .WithName("AddTask")
// .Produces<item>(StatusCodes.Status201Created)
// .Produces(StatusCodes.Status400BadRequest); // הוספת אפשרות לתגובה רעה

// Route לעדכון משימה
app.MapPatch("/tasks/{Id}/{IsComplete}", async (ToDoDbContext db, int Id, bool IsComplete) =>
{
    
    var item = await db.item.FindAsync(Id);

    if (item is null) return Results.NotFound();


    // item.Name = updateditem.Name; // עדכון שדות בהתאם למודל שלך
    // item.IsComplete = updateditem.IsComplete; // עדכון השם ל-IsComplete
    item.IsComplete=IsComplete;


    await db.SaveChangesAsync();
    return Results.Ok();
});
// .WithName("UpdateTask")
// .Produces(StatusCodes.Status204NoContent)
// .Produces(StatusCodes.Status404NotFound);

// Route למחיקת משימה
app.MapDelete("/tasks/{id}", async (ToDoDbContext db, int id) =>
{
    
    var item = await db.item.FindAsync(id);

    if (item is null) return Results.NotFound();

    db.item.Remove(item);
    await db.SaveChangesAsync();
    return Results.Ok();
})
.WithName("DeleteTask")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.Run();
