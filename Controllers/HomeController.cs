using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TutorialWeb.Models;
using TutorialWeb.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TutorialWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAIModelAsync _aiModel;

    public HomeController(ILogger<HomeController> logger, IAIModelAsync aiModel)
    {
        _logger = logger;
        _aiModel = aiModel;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult EmojiTranslation()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> GenerateEmoji(string inputText)
    {
        string emojiResult = await _aiModel.GenerateAIResponseAsync(inputText);
        return Json(new { result = emojiResult });
    }

    public IActionResult Tools()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

