using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// In-memory store for demonstration
var items = new ConcurrentDictionary<int, string>();
var idCounter = 1;

async Task ForwardLogs(object logObj)
{
    try
    {
        using var client = new HttpClient();
        var logServer = Environment.GetEnvironmentVariable("LOKI_SERVER")  ?? "http://loki:3100";
        var logUrl = $"{logServer}/loki/api/v1/push";
        var logJson = JsonSerializer.Serialize(logObj);
        var payload = JsonSerializer.Serialize(new {
            streams = new[] {
                new {
                    stream = new { app = "backend" },
                    values = new[] { new[] { ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds() + "000000", logJson } }
                }
            }
        });
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        await client.PostAsync(logUrl, content);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error forwarding log: {ex.Message}");
    }
}

// GET: /items
app.MapGet("/items", async () => {
    await ForwardLogs(new { level = "info", action = "get_items", items = items });
    return Results.Ok(items);
});

// GET: /items/{id}
app.MapGet("/items/{id:int}", async (int id) =>
{
    var found = items.TryGetValue(id, out var value);
    await ForwardLogs(new { level = found ? "info" : "warn", action = "get_item", id, value });
    return found
        ? Results.Ok(new { id, value })
        : Results.NotFound();
});

// POST: /items
app.MapPost("/items", async (HttpRequest request) =>
{
    var value = await request.ReadFromJsonAsync<string>();
    if (string.IsNullOrEmpty(value))
    {
        await ForwardLogs(new { level = "error", action = "post_item", error = "Value is required." });
        return Results.BadRequest("Value is required.");
    }

    var id = idCounter++;
    items[id] = value;
    await ForwardLogs(new { level = "info", action = "post_item", id, value });
    return Results.Created($"/items/{id}", new { id, value });
});

// PUT: /items/{id}
app.MapPut("/items/{id:int}", async (int id, HttpRequest request) =>
{
    if (!items.ContainsKey(id))
    {
        await ForwardLogs(new { level = "warn", action = "put_item", id, error = "Not found" });
        return Results.NotFound();
    }

    var value = await request.ReadFromJsonAsync<string>();
    if (string.IsNullOrEmpty(value))
    {
        await ForwardLogs(new { level = "error", action = "put_item", id, error = "Value is required." });
        return Results.BadRequest("Value is required.");
    }

    items[id] = value;
    await ForwardLogs(new { level = "info", action = "put_item", id, value });
    return Results.Ok(new { id, value });
});

// DELETE: /items/{id}
app.MapDelete("/items/{id:int}", async (int id) =>
{
    var removed = items.TryRemove(id, out _);
    await ForwardLogs(new { level = removed ? "info" : "warn", action = "delete_item", id });
    return removed
        ? Results.NoContent()
        : Results.NotFound();
});

// Health check endpoints
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));
app.MapGet("/ready", () => Results.Ok(new { status = "ready", timestamp = DateTime.UtcNow }));

app.Run();