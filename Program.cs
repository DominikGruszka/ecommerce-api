using Microsoft.OpenApi.Models;
using EcommerceApi.Data;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Dodanie usług do kontenera
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecommerce API", Version = "v1" });
});

// Rejestracja DbContexta **przed** budową aplikacji
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseSqlite("Data Source=ecommerce.db"));

var app = builder.Build();

app.MapGet("/products", async (EcommerceDbContext db) =>
    await db.Products.ToListAsync());

// Pobranie produktów po ID
app.MapGet("/products/{id:int}", async (int id, EcommerceDbContext db) =>
    await db.Products.FindAsync(id)
        is Product product
            ? Results.Ok(product)
            : Results.NotFound());

// Dodanie nowego produktu 
app.MapPost("/products", async (Product product, EcommerceDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

// Aktualizacja produktu 
app.MapPut("/products/{id:int}", async (int id, Product inputProduct, EcommerceDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product == null) return Results.NotFound();

    product.Name = inputProduct.Name;
    product.Description = inputProduct.Description;
    product.Price = inputProduct.Price;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Usunięcie produktu
app.MapDelete("/products/{id:int}", async (int id, EcommerceDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product == null) return Results.NotFound();

    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
// Pobranie wszystkich zamówień 
app.MapGet("/orders", async (EcommerceDbContext db) =>
    await db.Orders.Include(o => o.Products).ToListAsync());

// Pobranie zamówień po ID
app.MapGet("/orders/{id:int}", async (int id, EcommerceDbContext db) =>
    await db.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == id)
        is Order order
            ? Results.Ok(order)
            : Results.NotFound());

// Dodanie nowych zamówień 
app.MapPost("/orders", async (Order order, EcommerceDbContext db) =>
{
    var products = new List<Product>();
    foreach (var p in order.Products)
    {
        var product = await db.Products.FindAsync(p.Id);
        if (product != null)
            products.Add(product);
    }
    order.Products = products;

    db.Orders.Add(order);
    await db.SaveChangesAsync();
    return Results.Created($"/orders/{order.Id}", order);
});

// Aktualizaowanie zamówień 
app.MapPut("/orders/{id:int}", async (int id, Order inputOrder, EcommerceDbContext db) =>
{
    var order = await db.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == id);
    if (order == null) return Results.NotFound();

    // Aktualizowanie listy produktów
    var products = new List<Product>();
    foreach (var p in inputOrder.Products)
    {
        var product = await db.Products.FindAsync(p.Id);
        if (product != null)
            products.Add(product);
    }
    order.Products = products;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Usuwanie zamówień
app.MapDelete("/orders/{id:int}", async (int id, EcommerceDbContext db) =>
{
    var order = await db.Orders.FindAsync(id);
    if (order == null) return Results.NotFound();

    db.Orders.Remove(order);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


// Konfiguracja potoku HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API v1");
    });
}

app.UseHttpsRedirection();


// Przykładowy endpoint GET (do testu Swaggera)
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
