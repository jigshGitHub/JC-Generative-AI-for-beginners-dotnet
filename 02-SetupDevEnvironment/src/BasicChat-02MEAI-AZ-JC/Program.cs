using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;

var deploymentName = "JC-Learn-gpt-4"; // e.g. "gpt-4o-mini"
var endpoint = new Uri("https://jcazoaisvc506960901982.openai.azure.com/");
var apiKey = new ApiKeyCredential(Environment.GetEnvironmentVariable("JC_AZURE_AI_KEY"));

IChatClient client = new AzureOpenAIClient(
    endpoint,
    apiKey)
.AsChatClient(deploymentName);

var response = await client.GetResponseAsync("What is AI?");

Console.WriteLine(response.Message);