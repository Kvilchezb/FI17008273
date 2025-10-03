# Práctica Programada 2 - Programación Avanzada en Web

**Nombre:** Kyran Jose Vilchez Barrantes  
**Carné:** FI17008273

---

## ⚙️ Comandos dotnet utilizados (CLI)


dotnet new sln -n PP2
dotnet new mvc -n BinaryOperationsApp
dotnet sln add BinaryOperationsApp/BinaryOperationsApp.csproj
dotnet run --project BinaryOperationsApp


**Fuentes consultadas**

https://www.programmersought.com/article/23481390219
https://www.programiz.com/csharp-programming/bitwise-operators
https://www.youtube.com/watch?v=D-ARX1GORQY

**Prompts de IA**
Prompt:

¿Cómo puedo validar cadenas binarias en ASP.NET Core MVC y mostrar los resultados de operaciones bitwise en una tabla con estilo?

Respuesta de GPT-4.1:

Puedes usar DataAnnotations para validar que las cadenas solo contengan 0 y 1, tengan longitud entre 2 y 8, y sean múltiplos de 2. Luego puedes implementar métodos personalizados para realizar operaciones bitwise como AND, OR y XOR iterando sobre los caracteres de las cadenas. Para mostrar los resultados, puedes usar Razor y aplicar estilos CSS para mejorar la presentación.

##return $@"
<style>
    table {{
        border-collapse: collapse;
        width: 100%;
        margin-top: 20px;
        font-family: Arial, sans-serif;
    }}
    th, td {{
        border: 1px solid #333;
        padding: 8px;
        text-align: center;
    }}
    th {{
        background-color: #f2f2f2;
    }}
    tr:nth-child(even) {{
        background-color: #f9f9f9;
    }}
</style> ##


**Preguntas**
1. ¿Cuál es el número que resulta al multiplicar, si se introducen los valores máximos permitidos en a y b?
Valores máximos permitidos:

a = 11111111 (255 en decimal)
b = 11111111 (255 en decimal)
Resultado: 255 * 255 = 65025

Representaciones:

Binaria: 1111111000000001
Octal: 177401
Decimal: 65025
Hexadecimal: FE01

2. ¿Es posible hacer las operaciones en otra capa? Si sí, ¿en cuál sería?
Sí as operaciones pueden realizarse en una clase de servicio dentro de una carpeta "Services", lo que permite separar la lógica del controlador y facilita la reutilización y pruebas unitarias. También podrían colocarse en el Model, si se desea encapsular la lógica directamente con los datos.
