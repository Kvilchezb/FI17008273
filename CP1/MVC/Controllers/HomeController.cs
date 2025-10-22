using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(TheModel model)
    {
        ViewBag.Valid = ModelState.IsValid;
        if (ViewBag.Valid)
        {
            var cleanedChars = model.Phrase.Where(c => !Char.IsWhiteSpace(c));

            model.Counts = cleanedChars
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            model.Lower = new string(cleanedChars.Select(c => Char.ToLower(c)).ToArray());
            model.Upper = new string(cleanedChars.Select(c => Char.ToUpper(c)).ToArray());
        }
        return View(model);
    }
}
