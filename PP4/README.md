
# Práctica Programada 4 - Programación Avanzada en Web (SC-701)

**Estudiante:** Kyran Jose Vilchez Barrantes
**Carné:** FI17008273

---

## ⚙️ Comandos utilizados
```bash
mkdir PP4
cd PP4
dotnet new sln -n PP4
dotnet new console -n MyProject
dotnet sln add MyProject/MyProject.csproj
cd MyProject
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

---

## 📚 Fuentes de ayuda
- Documentación oficial de .NET y EF Core
- Copilot para explicación de conceptos y generación de código
- https://learn.microsoft.com/es-es/dotnet/
- https://learn.microsoft.com/es-es/ef/core/


---
### 💬 Prompts y respuestas de IA

**Usuario:** Me ayudas a corregir este código que realicé por mi cuenta, me sale varios errores,esta práctica programada 4

**Copilot:** Revisé el documento y resumí lo siguiente...

**Agregar los paquetes necesarios**

```sh
cd MyProject
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
```

**Usuario:** Sí, empecemos paso a paso...

**Copilot:** Pasos iniciales: crear solución y proyecto...

```sh
mkdir PP4
cd PP4
dotnet new sln -n PP4
dotnet new console -n MyProject
dotnet sln add MyProject/MyProject.csproj
mkdir data
```

**Usuario:** Puedes verificar el CSV?...

**Copilot:** Procesé el CSV: 714 registros...

**Usuario:** Sí avancemos...

**Copilot:** Creamos las clases Author, Title, Tag, TitleTag...

```sh
using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        public string AuthorName { get; set; }

        public ICollection<Title> Titles { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class Title
    {
        [Key]
        public int TitleId { get; set; }

        [Required]
        public string TitleName { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        public ICollection<TitleTag> TitleTags { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required]
        public string TagName { get; set; }

        public ICollection<TitleTag> TitleTags { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class TitleTag
    {
        [Key]
        public int TitleTagId { get; set; }

        [Required]
        public int TitleId { get; set; }

        [Required]
        public int TagId { get; set; }

        [ForeignKey("TitleId")]
        public Title Title { get; set; }

        [ForeignKey("TagId")]
        public Tag Tag { get; set; }
    }
}
```


**Usuario:** Sí avancemos, ya tenia varias partes de ese código...

**Copilot:** Agregamos lógica para generar archivos TSV...

```sh
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using MyProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        using var context = new AppDbContext();

        if (!context.Authors.Any())
        {
            Console.WriteLine("La base de datos está vacía, por lo que será llenada a partir de los datos del archivo CSV.");
            Console.WriteLine("Procesando...");

            var lines = File.ReadAllLines("./data/books.csv").Skip(1); // omitir encabezado

            foreach (var line in lines)
            {
                var parts = SplitCsvLine(line);
                if (parts.Length < 3) continue;

                var authorName = parts[0].Trim('"');
                var titleName = parts[1].Trim('"');
                var rawTags = parts[2].Split('\n');

                // Insertar autor si no existe
                var author = context.Authors.FirstOrDefault(a => a.AuthorName == authorName);
                if (author == null)
                {
                    author = new Author { AuthorName = authorName };
                    context.Authors.Add(author);
                    context.SaveChanges();
                }

                // Insertar título
                var title = new Title { TitleName = titleName, AuthorId = author.AuthorId };
                context.Titles.Add(title);
                context.SaveChanges();

                // Insertar etiquetas
                foreach (var tagName in rawTags)
                {
                    var cleanTag = tagName.Trim();
                    if (string.IsNullOrEmpty(cleanTag)) continue;

                    var tag = context.Tags.FirstOrDefault(t => t.TagName == cleanTag);
                    if (tag == null)
                    {
                        tag = new Tag { TagName = cleanTag };
                        context.Tags.Add(tag);
                        context.SaveChanges();
                    }

                    var titleTag = new TitleTag { TitleId = title.TitleId, TagId = tag.TagId };
                    context.TitlesTags.Add(titleTag);
                    context.SaveChanges();
                }
            }

            Console.WriteLine("Listo.");
        }
        else
        {
            Console.WriteLine("La base de datos se está leyendo para crear los archivos TSV.");
            Console.WriteLine("Procesando...");

            var query = from tt in context.TitlesTags
                        join t in context.Titles on tt.TitleId equals t.TitleId
                        join a in context.Authors on t.AuthorId equals a.AuthorId
                        join tg in context.Tags on tt.TagId equals tg.TagId
                        select new
                        {
                            AuthorName = a.AuthorName,
                            TitleName = t.TitleName,
                            TagName = tg.TagName
                        };

            var grouped = query
                .AsEnumerable()
                .GroupBy(x => x.AuthorName[0].ToString().ToUpper());

            foreach (var group in grouped)
            {
                var filePath = $"./data/{group.Key}.tsv";
                using var writer = new StreamWriter(filePath);

                writer.WriteLine("AuthorName\tTitleName\tTagName");

                foreach (var item in group
                    .OrderByDescending(x => x.AuthorName)
                    .ThenByDescending(x => x.TitleName)
                    .ThenByDescending(x => x.TagName))
                {
                    writer.WriteLine($"{item.AuthorName}\t{item.TitleName}\t{item.TagName}");
                }
            }

            Console.WriteLine("Listo.");
        }
    }

    static string[] SplitCsvLine(string line)
    {
        // Divide la línea CSV respetando comas dentro de comillas
        return Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
    }
}
```

**Usuario:** Perfecto, me regalas el script de la conversación...

**Copilot:** Aquí está el script completo para tu README.md...

---

## ❓ Preguntas teóricas

**¿Cómo cree que resultaría el uso de la estrategia de Code First para crear y actualizar una base de datos de tipo NoSQL (como por ejemplo MongoDB)? ¿Y con Database First? ¿Cree que habría complicaciones con las Foreign Keys?**

Si se usa el método Code First con una base de datos NoSQL, como MongoDB, el trabajo se hace difícil. Esto pasa porque el método se hizo para bases de datos con tablas. Esos sistemas tienen claves y un esquema fijo. 

MongoDB usa documentos que cambian fácil. No hay un plan estricto. No hay pasos automáticos. Para hacer algo parecido, hacen falta libros de código para NoSQL. Se debe cambiar mucha lógica a mano. 

El método Database First tampoco ayuda mucho. Este plan necesita un esquema ya puesto. Así puede hacer las clases solas. En NoSQL, ese plan no existe. Esto detiene el hacer modelos de forma libre. 

Además, las claves externas son un problema grande. NoSQL no usa esos lazos viejos de bases relacionales. En su lugar, se usan números de identificación o se meten documentos dentro de otros. Esto deja sin sentido la regla de las claves externas.

**¿Cuál carácter, además de la coma (,) y el Tab (\t), se podría usar para separar valores en un archivo de texto con el objetivo de ser interpretado como una tabla (matriz)? ¿Qué extensión le pondría y por qué? Por ejemplo: Pipe (|) con extensión .pipe.**

Un buen indicio para separar datos, además de la coma y el espacio, es la línea vertical (|). Este signo casi nunca aparece en los datos. Es fácil de ver y no choca con otros separadores. Para nombrar este tipo de archivo, se puede usar la extensión . pipe. Esto muestra bien cuál es el separador. Así no hay lío con ficheros . csv o . tsv.

Ejemplo:
Autor|Título|Tema"Acuña, Manuel"|Selección de versos|Letras de habla hispana

---

