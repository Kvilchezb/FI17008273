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