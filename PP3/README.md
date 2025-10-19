# Práctica Programada 3 - PP3

**Curso:** Programación Avanzada en Web  
**Código:** SC-701  
**Profesor:** Luis Andrés Rojas Matey  
**Estudiante:** Kyran Jose Vilchez Barrantes  
**Carné:** FI17008273  
**Fecha de entrega:** Martes 21 de octubre antes de las 6 pm

---

## 🧩 Objetivo
Aplicar los conocimientos adquiridos al utilizar un Minimal API con la herramienta ASP.NET Core Minimal API del Framework .NET 8.0.

---

## 🛠️ Comandos `dotnet` utilizados
```bash
mkdir PP3
cd PP3
dotnet new sln -n PP3
dotnet new webapi -n TextApi --framework net8.0
dotnet sln PP3.sln add TextApi/TextApi.csproj
dotnet add package Swashbuckle.AspNetCore
dotnet run
```

---

## 🌐 Páginas web consultadas
- https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api
- https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new
- https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started
- https://stackoverflow.com/

---

## 🤖 Prompts y respuestas de IA utilizadas
- **Copilot Chat:**
  - Prompt: "Me puedes dar el código completo para solucionarlo"
  - Respuesta: Código completo de `Program.cs` con todos los endpoints y Swagger.
  
- **ChatGPT:**
  - Prompt: "¿Cómo agrego Swagger a un proyecto Minimal API en .NET 8.0?"
  - Respuesta: Instrucciones para instalar `Swashbuckle.AspNetCore` y configurar Swagger UI.

---

## ❓ Preguntas del enunciado

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

---

## 📁 Estructura del repositorio
```
PP3/
├── TextApi/
│   ├── Program.cs
│   ├── TextApi.csproj
├── PP3.sln
├── README.md
```

---

## ✅ Notas finales
- Se excluyeron las carpetas `bin/` y `obj/` usando el archivo `.gitignore` del repositorio del profesor.
- El proyecto fue probado con Postman y Swagger UI.
- Todos los endpoints cumplen con las especificaciones funcionales y técnicas del enunciado.

