using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// In-memory store for demonstration
var items = new ConcurrentDictionary<int, string>();
var idCounter = 1;

// GET: /items
app.MapGet("/items", () => Results.Ok(items));

// GET: /items/{id}
app.MapGet("/items/{id:int}", (int id) =>
    items.TryGetValue(id, out var value)
        ? Results.Ok(new { id, value })
        : Results.NotFound()
);

// POST: /items
app.MapPost("/items", async (HttpRequest request) =>
{
    Console.WriteLine(request);
    var value = await request.ReadFromJsonAsync<string>();
    if (string.IsNullOrEmpty(value))
        return Results.BadRequest("Value is required.");

    var id = idCounter++;
    items[id] = value;
    return Results.Created($"/items/{id}", new { id, value });
});

// PUT: /items/{id}
app.MapPut("/items/{id:int}", async (int id, HttpRequest request) =>
{
    if (!items.ContainsKey(id))
        return Results.NotFound();

    var value = await request.ReadFromJsonAsync<string>();
    if (string.IsNullOrEmpty(value))
        return Results.BadRequest("Value is required.");

    items[id] = value;
    return Results.Ok(new { id, value });
});

// DELETE: /items/{id}
app.MapDelete("/items/{id:int}", (int id) =>
{
    if (items.TryRemove(id, out _))
        return Results.NoContent();
    return Results.NotFound();
});

app.Run();