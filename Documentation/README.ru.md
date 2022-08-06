[← Назад](https://github.com/AnatoliyCh/bot-get-by-link)

# Введение

Проект, разработан в учебных целях, представляет собой бота, который возвращает контент в «чистом» виде по указанной ссылке.

Поддерживаются ссылки на сервисы:

-   `Reggit` (ограничение: видео без звука) (**указать перечень шаблонов ссылок**);
-   `Vkontakte/Vk` (только посты со стены) (**указать перечень шаблонов ссылок**);

Поддерживаемые клиенты:

-   [`Telegram`][1] (**указать ссылку на бота**);

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
Происк проблем в коде:

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
