using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using DotNetEnv;
using System.Text;


Env.Load();
Console.WriteLine(Environment.GetEnvironmentVariable("JC_AZ_DEPLOYMENT_NAME"));
Console.WriteLine(Environment.GetEnvironmentVariable("JC_AZ_ENDPOINT"));

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

Console.WriteLine(apiKey);
var apiKeyCredential = new ApiKeyCredential(apiKey);

IChatClient client = new AzureOpenAIClient(
    endpoint,
    apiKeyCredential)
.AsChatClient(deploymentName);

StringBuilder prompt = new StringBuilder();
prompt.AppendLine("You will analyze the sentiment of the following product reviews. Each line is its own review. Output the sentiment of each review in a bulleted list and then provide a generate sentiment of all reviews. ");
prompt.AppendLine("I bought this product and it's amazing. I love it!");
prompt.AppendLine("This product is terrible. I hate it.");
prompt.AppendLine("I'm not sure about this product. It's okay.");
prompt.AppendLine("I found this product based on the other reviews. It worked for a bit, and then it didn't.");

// send the prompt to the model and wait for the text completion
var response = await client.GetResponseAsync(prompt.ToString());

// display the response
Console.WriteLine(response.Message);