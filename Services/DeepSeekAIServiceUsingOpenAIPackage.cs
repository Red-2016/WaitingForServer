using TutorialWeb.Interfaces;
using System.ClientModel;
using Microsoft.Extensions.AI;
using TutorialWeb.Models;
using Microsoft.Extensions.Options;

namespace TutorialWeb.Services;

public class DeepSeekAIServiceUsingOpenAIPackage : IAIModelAsync
{
    private string apiKey;

    private string systemPrompt = "你是一个能够将文本转换为emoji表达的助手。请根据以下提供的句子和对应的emoji翻译，学习并应用相同的规则来翻译新的句子。\n 样本：\n人工智能正在改变我们的生活，未来会有更多创新。\n🤖➡️🔃👨‍👩‍👧‍👦🏠🌍，🕙⏩➡️➕💡🚀🔮。\n昨晚我和张三一起吃了火锅，聊了很多有趣的话题，感觉时间过得很快。\n⬅️⏲️🌃🧑↔️👨‍🦱=\"张三\"🥢🍲，🧑↔️👫🗣️💭😄，🧠➡️⏳🕙⏩。\n站在泰山山顶，看着远处的云和海，感觉整个世界都安静了。\n🧑🔽⛰️=\"泰山\"，👀➡️🏞️☁️🌊，🧠💭➡️🌍➡️🤫。\n每天坚持锻炼身体，不仅能保持健康，还能让心情变好。\n🗓️➡️🏋️‍♂️，➡️🧑💪，➡️🧑💭😊。\n学习一门新语言需要时间和耐心，但最终会很有成就感。\n🧠📖➡️🗣️💬📚➡️➕⏳➕😌，🕙⏭️🧑➡️😆。\n今天的会议很顺利，大家都提出了很多好建议。\n🗓️🔽💼📊💭➡️✅，👥➡️💡💭✅。\n今天天气很热，阳光特别强烈，记得多喝水。\n🗓️🔽🌤️➡️🌡️，☀️➡️🔥，🧠💭➡️👄➕🥤💧。\n谢谢你一直以来的支持，你是我最好的朋友。\n🙏➡️👨💪💖➡️🧑，👨↔️🧑🟰👫🙌🤝。\n你只需要返回翻译后的emoji语句，不需要回答其他内容。";


    public DeepSeekAIServiceUsingOpenAIPackage(IOptions<AIConfig> config)
    {
        apiKey = config.Value.ApiKey;
    }

    public async Task<string> GenerateAIResponseAsync(string inputText)
    {
        OpenAIChatClient openAIChatClient = new OpenAIChatClient(

        new OpenAI.OpenAIClient(new ApiKeyCredential(apiKey),

        new OpenAI.OpenAIClientOptions() { Endpoint = new Uri("https://api.deepseek.com") }), "deepseek-chat"); 
        var chatMessages = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, systemPrompt),

            new ChatMessage(ChatRole.User, inputText)
        }; 

        var reply = await openAIChatClient.CompleteAsync(chatMessages, new ChatOptions() { }); 
        System.Console.WriteLine(reply.Message);
        return reply.Choices[0].Text;
    }
}


