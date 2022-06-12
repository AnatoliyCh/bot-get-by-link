using System.Reflection;
using Bot.GetByLink.Client.Telegram.Polling;
using Microsoft.Extensions.Configuration;

// get project name
var projectName = Assembly.GetExecutingAssembly().GetName().Name;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true, true)
    .AddEnvironmentVariables().Build();

configuration["project-name"] = projectName;

var client = new ClientPolling(configuration);

var startMessage = $"{projectName}: start";

var launched = await client.Start();

if (launched)
{
    Console.WriteLine(startMessage);
    await client.SendTextMessageToLogChatAsync(startMessage);
}

var looping = true;

while (looping)
{
    Console.Write("Command: ");
    var command = Console.ReadLine();
    switch (command)
    {
        case "exit polling":
            looping = false;
            client.Stop();
            break;
        case "start polling":
            Console.WriteLine($"{projectName}: starting polling...");
            await client.Start();
            break;
        case "stop polling":
            Console.WriteLine($"{projectName}: stoping polling...");
            client.Stop();
            break;
    }
}