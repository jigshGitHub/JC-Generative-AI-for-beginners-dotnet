using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using DotNetEnv;
using System.Text;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;

Env.Load();
var deploymentName = Environment.GetEnvironmentVariable("JC_AZ_DEPLOYMENT_NAME");
if (string.IsNullOrEmpty(deploymentName))
{
    throw new InvalidOperationException("Missing JC_AZ_DEPLOYMENT_NAME environment variable. Ensure you followed the instructions to setup a GitHub Token to use GitHub Models.");
}
var endpointName = Environment.GetEnvironmentVariable("JC_AZ_ENDPOINT");
if (string.IsNullOrEmpty(endpointName))
{
    throw new InvalidOperationException("Missing JC_AZ_ENDPOINT environment variable. Ensure you followed the instructions to setup a GitHub Token to use GitHub Models.");
}
var endpoint = new Uri(endpointName);
if (string.IsNullOrEmpty(endpointName))
{
    throw new InvalidOperationException("Missing JC_AZ_ENDPOINT environment variable. Ensure you followed the instructions to setup a GitHub Token to use GitHub Models.");
}
var apiKey = Environment.GetEnvironmentVariable("JC_AZURE_AI_KEY");
if (string.IsNullOrEmpty(apiKey))
{
    throw new InvalidOperationException("Missing JC_AZURE_AI_KEY environment variable. Ensure you followed the instructions to setup a GitHub Token to use GitHub Models.");
}
var apiKeyCredential = new ApiKeyCredential(apiKey);

// create client
var client = new AzureOpenAIClient(
    endpoint,
    apiKeyCredential);
    
// Create a chat completion service
var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(deploymentName, client);

// Get the chat completion service
Kernel kernel = builder.Build();
var chat = kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();
history.AddSystemMessage("You are a useful chatbot. If you don't know an answer, say 'I don't know!'. Always reply in a funny way. Use emojis if possible.");

while (true)
{
    Console.Write("Q: ");
    var userQ = Console.ReadLine();
    if (string.IsNullOrEmpty(userQ))
    {
        break;
    }
    history.AddUserMessage(userQ);

    var sb = new StringBuilder();
    var result = chat.GetStreamingChatMessageContentsAsync(history);
    Console.Write("AI: ");
    await foreach (var item in result)
    {
        sb.Append(item);
        Console.Write(item.Content);
    }
    Console.WriteLine();

    history.AddAssistantMessage(sb.ToString());
}
