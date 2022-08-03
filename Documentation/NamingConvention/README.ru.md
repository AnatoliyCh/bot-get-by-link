[← Назад](https://github.com/AnatoliyCh/bot-get-by-link/blob/change-documentation/README.md#documentation)

# Введение

Данный гайд опирается на официальные [рекомендации от Microsoft][1]. В основном используются `PascalCase` и `camelCase` стили наименования.  
Для именования **git-веток** используется стиль наименования `kebab-case`.

Навигация:

-   [Папки и директории][2];
-   [Элементы языка][3];
-   [Советы по коду][4];

## Папки и директории

| Объект                       | Рекомендация                                             | Пример                                           |
| ---------------------------- | -------------------------------------------------------- | ------------------------------------------------ |
| Решение                      | MyCompany.MyTechnology                                   | Bot.GetByLink                                    |
| Папка решения                | MyCompany.MyTechnology.TotalName                         | Bot.GetByLink.Common                             |
| Проект                       | MyCompany.MyTechnology.FirstFeature                      | Bot.GetByLink.Common.Infrastructure              |
| Папка проекта                | PascalCase                                               | Abstractions                                     |
| Файл class / record / struct | PascalCase                                               | ProxyResponseContent.cs                          |
| Файл интерфейса              | IPascalCase                                              | IProxyContent.cs                                 |
| Файл конфигурации **.json**  | appsettings или PascalCase                               | appsettings.json                                 |
| Блоки и поля **.json**       | "PascalCase"                                             | "Clients": { "Telegram": { "Token": "", }, },    |
| Пространства имен            | MyCompany.(Product\|Technology)[.Feature][.subnamespace] | Bot.GetByLink.Common.Infrastructure.Abstractions |

## Элементы языка

У всех **объектов** языка _(класс, структура, запись, интерфейс и т.д.)_ **необходимо ЯВНО указывать модификатор доступа** у полей, методов/функций, свойств и т.д.  
Все `public` элементы (поля, методы/функции, свойства и т.д.) должны быть описаны: `/// <summary> Comment </summary>`.

#### Объекты языка

Названия параметров универсальных шаблонов имеет вид `T` или `TName`: `public abstract class AsyncCommand<TName>`, `public interface IMessageContext<TChatId, TText, TArtifact>`.  
Используется ковариантность (`out T`) и контравариантность (`in T`) параметров универсальных шаблонов: `public interface ICommandInvoker<in TCommandName>`.  
У конструктора **абстрактного класса** указывать модификатор доступа `protected`. При наличии хотябы 1 `abstract` элемента, класс невобходимо сделать **абстрактным**.

| Объект            | Рекомендация                             | Пример                                       |
| ----------------- | ---------------------------------------- | -------------------------------------------- |
| Класс / структура | public sealed class PascalCase           | public sealed class UrlRegexWrapper          |
| Класс-расширение  | public static class PascalCaseExtensions | public static class LoggingBuilderExtensions |
| Запись (`record`) | public sealed record PascalCase          | public sealed record Message                 |
| Интерфейс         | IPascalCase                              | public interface IMessageContext             |

#### Поля / свойства

Сортировка **полей** в файле по признаку доступности: `private readonly`, `private`, `public`, `protected`.  
Для разрешения `null-значения` используется `?`: `private CancellationTokenSource? cts;`.  
**Поле / свойство** только для чтения: `private readonly Type camelCase`, `public Type PascalCase { get; }`.

| Объект                                       | Рекомендация                          | Пример                               |
| -------------------------------------------- | ------------------------------------- | ------------------------------------ |
| Поле `private`                               | private Type camelCase                | private CancellationTokenSource? cts |
| Поле `private readonly`                      | private readonly Type camelCase       | private readonly ILogger logger      |
| Поле `public`                                | public Type PascalCase                | public ILogger Logger                |
| Поле `protected`                             | protected Type camelCase              | protected ILogger logger             |
| Свойство `public`                            | public Type PascalCase { get; set; }  | public string Url { get; set; }      |
| Свойство `public` c начальной инициализацией | public Type PascalCase { get; init; } | public string Url { get; init; }     |
| Свойство `public readonly`                   | public Type PascalCase { get; }       | public Status State { get; }         |

#### Функции / Методы

Сортировка **методов / функций** в файле по признаку доступности: `public`, `protected`, `private`.  
**Async-методы** должны всегда возвращать `Task<T>` (вместо **void** `Task`), так как может наблюдатся неоднозначность поведения.

| Объект                                | Рекомендация                                                                 | Пример                 | Возвращаемый тип     |
| ------------------------------------- | ---------------------------------------------------------------------------- | ---------------------- | -------------------- |
| Метод / функция                       | PascalCase                                                                   | Execute                | object \| null       |
| Метод **async/await**                 | PascalCaseAsync                                                              | ExecuteAsync           | Task<object \| null> |
| Метод с **try/catch**                 | TryPascalCase                                                                | TryGetValue            | object \| null       |
| Метод **async/await** + **try/catch** | TryPascalCaseAsync                                                           | TryExecuteCommandAsync | Task<object \| null> |
| Аргументы метода / функции            | PascalCase(type camelCasing, type camelCasing)                               | IsMatch(string url)    | object \| null       |
| Локальные переменные                  | PascalCase(type camelCasing, type camelCasing) { var/type camelCasing = ...} |                        |                      |

## Советы по коду

-   Абстрактные / бызовые классы называть общим именем без приписок по типу `Base`;
-   У классов указывать ключевое слово `sealed`: `public sealed class PascalCase`, при отсутвии наследования;
-   Неизменяемые данные лучше хранить в типе `record`;
-   Входящие аргументы в конструктор **ВСЕГДА** проверять на `null`, можно через метод: `ArgumentNullException.ThrowIfNull()`;
-   **По возможности** все аргументы конструктора и поля, которым они присваиваются, должны быть интерфейсом;
-   **Поля** лучше инкапсулировать и использовать вместо них свойства.

[1]: https://docs.microsoft.com/ru-ru/dotnet/standard/design-guidelines/naming-guidelines
[2]: https://github.com/AnatoliyCh/bot-get-by-link/blob/change-documentation/Documentation/NamingConvention/README.ru.md#папки-и-директории
[3]: https://github.com/AnatoliyCh/bot-get-by-link/blob/change-documentation/Documentation/NamingConvention/README.ru.md#элементы-языка
[4]: https://github.com/AnatoliyCh/bot-get-by-link/blob/change-documentation/Documentation/NamingConvention/README.ru.md#советы-по-коду
[5]: https://dillinger.io/
[6]: https://markdown-editor.andona.click/#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
