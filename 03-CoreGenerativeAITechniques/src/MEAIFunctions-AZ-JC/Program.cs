using System.ComponentModel;
using System.ClientModel;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using DotNetEnv;
using Azure.AI.Inference;

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


  

ChatOptions options = new ChatOptions
{
    Tools = [
        AIFunctionFactory.Create(GetTheWeather)
    ]
};


IChatClient client = new AzureOpenAIClient(
    endpoint,
    apiKeyCredential)
.AsChatClient(deploymentName).AsBuilder()
    .UseFunctionInvocation()
    .Build();

var responseOne = await client.GetResponseAsync("What is today's date", options); 
Console.WriteLine($"response: {responseOne}");

var question = "Do I need an umbrella today?";
Console.WriteLine($"question: {question}");
var response = await client.GetResponseAsync(question, options);
Console.WriteLine($"response: {response}");

[Description("Get the weather")]
static string GetTheWeather()
{
    var temperature = Random.Shared.Next(5, 20);
    var conditions = Random.Shared.Next(0, 1) == 0 ? "sunny" : "rainy";
    var weatherInfo = $"The weather is {temperature} degrees C and {conditions}.";
    Console.WriteLine($"\tFunction Call - Returning weather info: {weatherInfo}");
    return weatherInfo;
}