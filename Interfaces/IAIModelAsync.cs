namespace TutorialWeb.Interfaces;

public interface IAIModelAsync
{
    Task<string> GenerateAIResponseAsync(string inputText);
}