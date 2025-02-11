using System;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TutorialWeb.Interfaces;
using TutorialWeb.Models;

namespace TutorialWeb.Services;

public class DeepSeekAIService : IAIModel
{
    private string modelName;
    private string apiKey;
    private string apiUrl;

    public DeepSeekAIService(IOptions<AIConfig> config)
    {
        modelName = config.Value.ModelName;
        apiKey = config.Value.ApiKey;
        apiUrl = config.Value.ApiUrl;
        Console.WriteLine(modelName, apiKey, apiUrl);
    }

    public string GenerateAIResponse(string inputText)
    {
        string systemPrompt = "你是一个可以把句子翻译成emoji的ai，你只需要返回翻译后的emoji句子，不需要返回其他内容。";

        var requestBody = new
        {
            model = modelName,
            messages = new[]
            {
            new
            {
                role = "system",
                content = systemPrompt.Trim()
            },
            new
            {
                role = "user",
                content = inputText
            }
        },
            temperature = 0.3
        };

        string jsonData = JsonConvert.SerializeObject(requestBody);

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            StringContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = client.PostAsync(apiUrl, httpContent).Result;
                response.EnsureSuccessStatusCode();

                string responseBody = response.Content.ReadAsStringAsync().Result;
                return ExtractContent(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return e.Message;
            }
        }
    }

    public string ExtractContent(string jsonResponse)
    {
        try
        {
            JObject parsedResponse = JObject.Parse(jsonResponse);
            return (string)parsedResponse["choices"][0]["message"]["content"];
        }
        catch (Exception ex)
        {
            return "解析错误: " + ex.Message;
        }
    }
}


