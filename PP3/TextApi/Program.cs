using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PP3 API", Version = "v1" });
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PP3 API v1");
});


// Endpoint /
app.MapGet("/", () => Results.Redirect("/swagger"));

// Método para serializar a XML
IResult ToXml(Result result)
{
    var xmlSerializer = new XmlSerializer(typeof(Result), new XmlRootAttribute("Result"));
    using var stringWriter = new StringWriter();
    xmlSerializer.Serialize(stringWriter, result);
    return Results.Content(stringWriter.ToString(), "application/xml");
}

// Endpoint /include
app.MapPost("/include/{position}", (int position, HttpRequest request) =>
{
    string? value = request.Query["value"];
    string? text = request.Form["text"];
    bool xml = request.Headers.TryGetValue("xml", out var xmlHeader) && xmlHeader == "true";

    if (position < 0)
        return Results.BadRequest(new { error = "'position' must be 0 or higher" });
    if (string.IsNullOrWhiteSpace(value))
        return Results.BadRequest(new { error = "'value' cannot be empty" });
    if (string.IsNullOrWhiteSpace(text))
        return Results.BadRequest(new { error = "'text' cannot be empty" });

    var words = text.Split(' ').ToList();
    if (position >= words.Count)
        words.Add(value);
    else
        words.Insert(position, value);

    var result = new Result { Ori = text, New = string.Join(" ", words) };
    return xml ? ToXml(result) : Results.Json(result);
});

// Endpoint /replace
app.MapPut("/replace/{length}", (int length, HttpRequest request) =>
{
    string? value = request.Query["value"];
    string? text = request.Form["text"];
    bool xml = request.Headers.TryGetValue("xml", out var xmlHeader) && xmlHeader == "true";

    if (length <= 0)
        return Results.BadRequest(new { error = "'length' must be greater than 0" });
    if (string.IsNullOrWhiteSpace(value))
        return Results.BadRequest(new { error = "'value' cannot be empty" });
    if (string.IsNullOrWhiteSpace(text))
        return Results.BadRequest(new { error = "'text' cannot be empty" });

    var words = text.Split(' ').Select(w => w.Length == length ? value : w).ToList();
    var result = new Result { Ori = text, New = string.Join(" ", words) };
    return xml ? ToXml(result) : Results.Json(result);
});

// Endpoint /erase
app.MapDelete("/erase/{length}", (int length, HttpRequest request) =>
{
    string? text = request.Form["text"];
    bool xml = request.Headers.TryGetValue("xml", out var xmlHeader) && xmlHeader == "true";

    if (length <= 0)
        return Results.BadRequest(new { error = "'length' must be greater than 0" });
    if (string.IsNullOrWhiteSpace(text))
        return Results.BadRequest(new { error = "'text' cannot be empty" });

    var words = text.Split(' ').Where(w => w.Length != length).ToList();
    var result = new Result { Ori = text, New = string.Join(" ", words) };
    return xml ? ToXml(result) : Results.Json(result);
});

app.Run();

public class Result
{
    public string Ori { get; set; } = string.Empty;
    public string New { get; set; } = string.Empty;
}