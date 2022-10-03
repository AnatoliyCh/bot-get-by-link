[← Back](https://github.com/AnatoliyCh/bot-get-by-link)

# Introduction 

Project developed for educational purposes, is a bot that return content a "pure" form at the specified link.

Service links supported:
  
- [`Reddit`](https://www.reddit.com/)  

Supports link:
1. Regular posts https://www.reddit.com/r/*/comments/*  
2. Albums/Galleries https://www.reddit.com/gallery/*  
  
Supports forwarding from the Reddit mobile app.  
  
Some restrictions:   
1. Video without sound (planned to implement with sound).  

-   [`Vkontakte/Vk`](https://vk.com)  
  
Supports links:
1. Posts on the wall including album/photo/video/document:  
	1.1. https://vk.com/feed?w=wall-000000000_0;  
	1.2. https://vk.com/wall-000000000_0;  
	1.3. https://vk.com/wall-000000000_000?z=album-000000000_000000000;  
	1.4. https://vk.com/feed?z=photo0_000000000%2Falbum-0000000000_000000000%2Frev;  
	1.5. https://vk.com/wall-000000000_000?z=album-000000000_000000000.  
2. Documents:  
	2.1. https://vk.com/doc000000000_000000000. 
3. Video:  
	3.1. https://vk.com/video-000000000_000000000?list=000000000.  
4. Photo:  
	4.1. https://vk.com/feed?z=photo0_000000000%2Falbum-0000000000_000000000%2Frev.  

Supports forwarding from the VK mobile application.

Some restrictions:
1. Video and document is added as a link in the text.    
  
Supported Clients

-   [`Telegram`][1] [Link](https://t.me/BotGetByLink_bot);  
  
General features:  
1. If there is a lot of content in the post (example: a post with a large text), the bot will send it in parts and with an interval specified in the config;  
2. Does not store or collect data and statistics;  
3. If the post contains links to third-party resources (example: gif from [gfycat](https://gfycat.com/)), then the content can be inserted as a link to this content.  
There are plans to return content even from third-party resources (partially implemented for Reddit).  
  
Command List:  
 + /help - help and description of the bot;  
 + /chatInfo - information about the chat in which the command was entered.  

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
