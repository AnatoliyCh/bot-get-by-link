[← Back](https://github.com/AnatoliyCh/bot-get-by-link)

# Introduction 

Project developed for educational purposes, is a bot that return content a "pure" form at the specified link.

Service links supported:

-   `Reggit` (limitation: video without sound) (**specify a list of link templates**);
-   `Vkontakte/Vk` (only posts from the wall) (**specify a list of link templates**);

Supported Clients

-   [`Telegram`][1] (**link to bot**);

## Getting started with the bot

1.  Download repository;
2.  To initialize the tools, use the command:

```PowerShell
dotnet tool restore
```

3.  Rebuild solution.

## Formatting and linting

When setting up the project, I used [this article][2].

To maintain the "purity" of the code is used [ReSharper command line tools][3].  
Finding problems in the code:

```PowerShell
dotnet jb inspectcode Bot.GetByLink.sln
```

Code formatting according to uniform rules:

```PowerShell
dotnet jb cleanupcode Bot.GetByLink.sln
```

Code analysers and linting:

-   [StyleCopAnalyzers][4];
-   [SonarSource][5].

## Project features

-   [Dependency injection][6]. This [article][7] helped;
-   [Configuration][8]. This [article][9] helped;
-   [Logging][10].

[← Back](https://github.com/AnatoliyCh/bot-get-by-link)

[1]: https://github.com/TelegramBots/Telegram.Bot
[2]: https://dev.to/srmagura/c-linting-and-formatting-tools-in-2021-bna
[3]: https://www.jetbrains.com/help/resharper/ReSharper_Command_Line_Tools.html#run-resharper-command-line-tools
[4]: https://github.com/DotNetAnalyzers/StyleCopAnalyzers
[5]: https://github.com/SonarSource/sonar-dotnet
[6]: https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection
[7]: https://pradeepl.com/blog/dotnet/dependency-injection-in-net-core-console-application
[8]: https://www.nuget.org/packages/Microsoft.Extensions.Configuration
[9]: https://pradeepl.com/blog/dotnet/configuration-in-a-net-core-console-application
[10]: https://www.nuget.org/packages/Microsoft.Extensions.Logging
