[← Назад](https://github.com/AnatoliyCh/bot-get-by-link)

# Введение

Проект, разработан в учебных целях, представляет собой бота, который возвращает контент в «чистом» виде по указанной ссылке.

Поддерживаются ссылки на сервисы:

-   [`Reddit`](https://www.reddit.com/)   

Поддерживает ссылки: 
1. Обычные посты https://www.reddit.com/r/*/comments/*  
2. Альбомы/галереи https://www.reddit.com/gallery/*  

Поддерживает перессылку с мобильного приложения Reddit.

Некоторые ограничения: 
1. Видео без звука (в планах реализовать со звуком).
	
-   [`Vkontakte/Vk`](https://vk.com)  
 
Поддерживает ссылки: 
1. Посты на стене в том числе с альбомом/фото/видео/документом:  
	1.1. https://vk.com/feed?w=wall-000000000_0;  
	1.2. https://vk.com/wall-000000000_0;   
	1.3. https://vk.com/wall-000000000_000?z=album-000000000_000000000;  
	1.4. https://vk.com/feed?z=photo0_000000000%2Falbum-0000000000_000000000%2Frev;  
	1.5. https://vk.com/wall-000000000_000?z=album-000000000_000000000.  
2. Документы:  
	2.1. https://vk.com/doc000000000_000000000.  
3. Видео:  
	3.1. https://vk.com/video-000000000_000000000?list=000000000.  
4. Фото:  
	4.1. https://vk.com/feed?z=photo0_000000000%2Falbum-0000000000_000000000%2Frev.  

Поддерживает перессылку с мобильного приложения VK.  

Некоторые ограничения: 
1. Видео и документ добавляется в качестве ссылке в тексте.

Поддерживаемые клиенты:

-   [`Telegram`][1] [Ссылка](https://t.me/BotGetByLink_bot);

Общие особенности: 
1. Если в посте много контента (пример: пост с большим текстом) бот будет присылать его по частям и с промежутком указаный в конфиге;
2. Данные и статистику не хранит и не собирает;
3. Если пост содержит ссылки на стороние ресурсы (пример: гифка из [gfycat](https://gfycat.com/)), то контент может быть вставлен как ссылка на этот контент.   
В планах возвращать контент даже из стороних ресурсов (частично реализовано для Reddit).  
  
Список команд:  
 + /help - справка и описание бота;  
 + /chatInfo - информация о чате в котором ввели команду.  

## Начало работы с ботом

1.  Скачать репозиторий;
2.  Для инициализации инструментов, использовать команду:

```PowerShell
dotnet tool restore
```

3.  Пересобрать решение.

## Форматирование и линтинг

При настройке проекта использовалась [данная статья][2].

Для поддержания "чистоты" кода используется [ReSharper command line tools][3].  
Поиск проблем в коде:

```PowerShell
dotnet jb inspectcode Bot.GetByLink.sln
```

Форматирование кода по единым правилам:

```PowerShell
dotnet jb cleanupcode Bot.GetByLink.sln
```

Анализаторы кода и линтинг:

-   [StyleCopAnalyzers][4];
-   [SonarSource][5].

## Фичи проекта

-   [Dependency injection][6]. Помогла эта [статья][7];
-   [Configuration][8]. Помогла эта [статья][9];
-   [Logging][10].

[← Назад](https://github.com/AnatoliyCh/bot-get-by-link)

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
