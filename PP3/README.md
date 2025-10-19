# Práctica Programada 3 - PP3

**Curso:** Programación Avanzada en Web  
**Código:** SC-701  
**Profesor:** Luis Andrés Rojas Matey  
**Estudiante:** Kyran Jose Vilchez Barrantes  
**Carné:** FI17008273  
**Fecha de entrega:** Martes 21 de octubre antes de las 6 pm
---

## 🛠️ Comandos `dotnet` utilizados
```bash
mkdir PP3
cd PP3
dotnet new sln -n PP3
dotnet new webapi -n TextApi --framework net8.0
dotnet sln PP3.sln add TextApi/TextApi.csproj
dotnet add package Swashbuckle.AspNetCore
cd TextApi
dotnet run
dotnet run
```

---

## 🌐 Páginas web consultadas
- https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api
- https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new
- https://stackoverflow.com/

---

## 🤖 Prompts y respuestas de IA utilizadas


**Usuario:** Intenté compilar el proyecto pero me dio el error CS8803 sobre instrucciones de nivel superior. ¿Cómo lo soluciono?  
**IA:** Ese error ocurre cuando se declara una clase en medio de instrucciones de nivel superior. Mueve la clase 'Result' al final del archivo 'Program.cs', después de 'app.Run();'.

**Usuario:** Como puedo realizar el comando IResult ToXml(Result result), un poco mas eficiente?
**IA:** var xmlSerializer = new XmlSerializer(typeof(Result), new XmlRootAttribute("Result"));
    using var stringWriter = new StringWriter();
    xmlSerializer.Serialize(stringWriter, result);
    return Results.Content(stringWriter.ToString(), "application/xml");

**Usuario:** Cuales "if" me recomendarias utilizar en "app.MapPost("/include/{position}", (int position, HttpRequest request)" ?
**IA:** if (position < 0)
        return Results.BadRequest(new { error = "'position' must be 0 or higher" });
    if (string.IsNullOrWhiteSpace(value))
        return Results.BadRequest(new { error = "'value' cannot be empty" });
    if (string.IsNullOrWhiteSpace(text))
        return Results.BadRequest(new { error = "'text' cannot be empty" });
    var result = new Result { Ori = text, New = string.Join(" ", words) };
    return xml ? ToXml(result) : Results.Json(result);

**Usuario:** Ejecuté el comando 'dotnet run' pero el proyecto se quedó congelado en la terminal. ¿Es normal?  
**IA:** Sí, eso indica que la API está corriendo correctamente y está esperando peticiones. Puedes probar los endpoints usando Postman o visitar http://localhost:puerto/swagger para ver la interfaz de Swagger.

**Usuario:** Me dio un error de String en la parte "app.MapPut("/replace/{length}", (int length, HttpRequest request) =>" que me recomiendas en codigo?
**IA:** Te recomiendo aplicar lo siguiente:

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

**Usuario:** Puedo agregar "public class Result" al final del codigo en Program.cs? 

**IA:** Si, solo agrega este código correctamente :

public class Result
{
    public string Ori { get; set; } = string.Empty;
    public string New { get; set; } = string.Empty;
}

---

**TESTEO EN POST-MAN**

URL : http://localhost:5297/include/0?value=Hello
Body:  x-www-form-urlencoded
Key:  Text
Value: Prueba

Resultado: 
{
    "ori": "Prueba",
    "new": "Hello Prueba"
}

Nota: ejecuté el programa con la URL http://localhost:5297/swagger/index.html

---

## Preguntas del enunciado

### ¿Es posible enviar valores en el Body (por ejemplo, en el Form) del Request de tipo GET?
No, no es posible enviar valores en el cuerpo (Body) de una petición GET. Según el estándar HTTP, los datos en una solicitud GET deben enviarse a través de la URL, ya sea como parte de la ruta o como parámetros de consulta (query string). Aunque técnicamente algunos clientes permiten enviar un cuerpo en GET, los servidores suelen ignorarlo.

### ¿Qué ventajas y desventajas observa con el Minimal API si se compara con la opción de utilizar Controllers?
**Ventajas:**
- Menor cantidad de código y configuración.
- Ideal para microservicios y APIs pequeñas.
- Más rápido de implementar y mantener.

**Desventajas:**
- Menor organización en proyectos grandes.
- Difícil de escalar si se requiere separación de responsabilidades.
- Menos soporte para filtros, validaciones y convenciones avanzadas que sí están disponibles en Controllers.


