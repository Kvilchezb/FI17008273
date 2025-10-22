using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Lista compartida
var list = new List<object>();

// GET → Redirige a Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

// POST → Retorna la lista, en XML si el header xml=true
app.MapPost("/", (HttpRequest request) =>
{
    var xmlHeader = request.Headers["xml"].ToString().ToLower();
    var returnXml = xmlHeader == "true";

    if (returnXml)
    {
        var serializer = new XmlSerializer(typeof(List<object>));
        using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, list);
        return Results.Content(stringWriter.ToString(), "application/xml");
    }

    return Results.Ok(list);
});

// PUT → Agrega elementos a la lista
app.MapPut("/", ([FromForm] int quantity, [FromForm] string type) =>
{
    if (quantity <= 0)
        return Results.BadRequest(new { error = "'quantity' must be higher than zero" });

    if (type != "int" && type != "float")
        return Results.BadRequest(new { error = "'type' must be 'int' or 'float'" });

    var random = new Random();
    for (int i = 0; i < quantity; i++)
    {
        list.Add(type == "int" ? random.Next() : random.NextSingle());
    }

    return Results.Ok(new { message = $"{quantity} {type} values added." });
}).DisableAntiforgery();

// DELETE → Elimina elementos de la lista
app.MapDelete("/", ([FromForm] int quantity) =>
{
    if (quantity <= 0)
        return Results.BadRequest(new { error = "'quantity' must be higher than zero" });

    if (list.Count < quantity)
        return Results.BadRequest(new { error = "List does not contain enough elements to delete" });

    for (int i = 0; i < quantity; i++)
    {
        list.RemoveAt(0);
    }

    return Results.Ok(new { message = $"{quantity} items removed." });
}).DisableAntiforgery();

// PATCH → Limpia la lista
app.MapPatch("/", () =>
{
    list.Clear();
    return Results.Ok(new { message = "List cleared successfully" });
});

app.Run();