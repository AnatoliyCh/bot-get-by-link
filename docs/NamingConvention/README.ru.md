[← Назад](https://github.com/AnatoliyCh/bot-get-by-link)

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
| Файл конфигурации `.json`    | appsettings или PascalCase                               | appsettings.json                                 |
| Блоки и поля `.json`         | "PascalCase"                                             | "Clients": { "Telegram": { "Token": "", }, },    |
| Пространства имен            | MyCompany.(Product\|Technology)[.Feature][.subnamespace] | Bot.GetByLink.Common.Infrastructure.Abstractions |

## Элементы языка

У всех **объектов** языка _(класс, структура, запись, интерфейс и т.д.)_ **необходимо ЯВНО указывать модификатор доступа** у полей, методов/функций, свойств и т.д.  
Все `public` элементы (поля, методы/функции, свойства и т.д.) должны быть описаны: `/// <summary> Comment </summary>`.

Общая сортировка элементов:

1. поля;
2. конструктор (любой модификатор доступа) ;
3. свойства;
4. методы;

#### Объекты языка

Названия параметров универсальных шаблонов имеют вид `T` или `TPascalCase`: `public abstract class AsyncCommand<TName>`, `public interface IMessageContext<TChatId, TText, TArtifact>`.  
Используется ковариантность (`out T`) и контравариантность (`in T`) параметров универсальных шаблонов: `public interface ICommandInvoker<in TCommandName>`.  
У конструктора **абстрактного класса** указывать модификатор доступа `protected`. При наличии хотя бы 1 `abstract` элемента, класс необходимо сделать **абстрактным**.

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

Раскладывание кортежа: `var (first, second) = PascalCase()`;

| Объект                                          | Рекомендация                             | Пример                               |
| ----------------------------------------------- | ---------------------------------------- | ------------------------------------ |
| Поле `private`                                  | private Type camelCase                   | private CancellationTokenSource? cts |
| Поле `private readonly`                         | private readonly Type camelCase          | private readonly ILogger logger      |
| Поле `public`                                   | public Type PascalCase                   | public ILogger Logger                |
| Поле `protected`                                | protected Type camelCase                 | protected ILogger logger             |
| Свойство `public`                               | public Type PascalCase { get; set; }     | public string Url { get; set; }      |
| Свойство `public` c начальной инициализацией    | public Type PascalCase { get; init; }    | public string Url { get; init; }     |
| Свойство `public readonly`                      | public Type PascalCase { get; }          | public Status State { get; }         |
| Свойство `protected`                            | protected Type PascalCase { get; set; }  | protected string Url { get; set; }   |
| Свойство `protected` c начальной инициализацией | protected Type PascalCase { get; init; } | protected string Url { get; init; }  |
| Свойство `protected readonly`                   | protected Type PascalCase { get; }       | protected Status State { get; }      |

#### Функции / Методы

Сортировка **методов / функций** в файле по признаку доступности: `public`, `protected`, `private`.  
**Async-методы** должны всегда возвращать `Task<T>` (вместо **void** `Task`), так как может наблюдаться неоднозначность поведения.

Кортеж как возвращаемый тип **методов / функций**: `(Type First, Type Second) PascalCase()`.  
Для деконструкции `Type` в кортеж используется метод `Deconstruct`: `public void Deconstruct(out Type First, out Type Second)`.

| Объект                            | Рекомендация                                                                 | Пример                 | Возвращаемый тип     |
| --------------------------------- | ---------------------------------------------------------------------------- | ---------------------- | -------------------- |
| Метод / функция                   | PascalCase                                                                   | Execute                | object \| null       |
| Метод `async/await`               | PascalCaseAsync                                                              | ExecuteAsync           | Task<object \| null> |
| Метод с `try/catch`               | TryPascalCase                                                                | TryGetValue            | object \| null       |
| Метод `async/await` + `try/catch` | TryPascalCaseAsync                                                           | TryExecuteCommandAsync | Task<object \| null> |
| Аргументы метода / функции        | PascalCase(type camelCasing, type camelCasing)                               | IsMatch(string url)    | object \| null       |
| Локальные переменные              | PascalCase(type camelCasing, type camelCasing) { var/type camelCasing = ...} |                        |                      |

#### События / делегаты

События/делегаты всегда ссылаются на какое-то действие, которое происходит или уже произошло. Их следует называть с помощью глаголов, а время глагола (настоящее или прошедшее) будет указывать на время возникновения события: `Clicked`, `Painting`, `DroppedDown`.  
Классы аргументов события/делегата именуются с суффиксом `EventArgs` и обычно являются типом, производным от `System.EventArgs` (в новых версиях языка **это не обязательно**): `PascalCaseEventArgs : EventArgs`.  
При **вызове** событий и делегатов используется абсолютно одинаковый синтаксис вызова методов: `object?.Invoke()` или `object?.Invoke(this, PascalCaseEventArgs e)`.

Что выбрать `delegate` или `event`:

-   `delegate`: если нужен возвращаемый тип или вызов события из вне;
-   `event`: закрытый вызов (только объект, который его содержит) и обработка **подписки/отписки** через аксессоры `add/remove`.

---

Наименование типа `delegate` происходит c глаголом и суффиксом `EventHandler`.  
Обязательные параметр делегата это `object sender` (объект, который вызвал этот делегат), и любой возвращаемый тип.

Для `delegate` существует несколько стандартных типов:

-   `Action<T...T16>`. `<T...T16>` аргументы делегата. Возвращает `void`;
-   `Func<T1...T16, out TResult>`. `<T...T16>` аргументы делегата. Возвращает `TResult`;
-   `Predicate<T>`. `<T>` единственный аргумент делегата. Возвращает `bool`. Данный тип используется **для тестов одного значения**.

---

События могут быть объявлены дополнительно как `static`, `private`, `virtual`, `abstract`. Обработчики событий **вызываются синхронно**, если имеется несколько подписчиков.  
Для `async/await` методов-обработчиков лучше оборачивать в **лямбда-ассинхронные функции** и добавлять `try/catch` (из-за возвращаемого типа `void`): `object.Event =+ async (sender, eventArgs) => { await PascalCase() }`.

Тип `event` должен быть типом делегата (`PascalCaseEventHandler`с возвращаемым типом `void`) или стандартным типами:

-   `EventHandler`. Делегат для всех событий, которые не включают данные о событии. При вызове можно использовать `EventArgs.Empty`. Метод слушателя: `public void OnProgress(object sender, EventArgs e) { ... }`;
-   `EventHandler<TEventArgs>`. Для событий, которые включают данные для отправки обработчикам. Метод слушателя для `EventHandler<bool>`: `public void OnProgress(object sender, bool IsSuccessful) { ... }`;

Имя метода обработчика события (тот метод, который подписываем) обычно содержит имя события с префиксом `On`:
`EventHandler / EventHandler<TEventArgs> / PascalCaseEventHandler OnProgress(object sender, EventArgs / TEventArgs / PascalCaseEventArgs e) { ... }`.

| Объект                                                      | Рекомендация                                                                                              | Пример                                                                                             |
| ----------------------------------------------------------- | --------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------- |
| Делегат                                                     | public delegate void PascalCaseEventHandler(object sender, PascalCaseEventArgs e)                         | public delegate void ClickedEventHandler(object sender, ClickedEventArgs count)                    |
| Событие с собственным типом делегата                        | public event `PascalCaseEventHandler` PascalCase                                                          | public event `ClickedEventHandler` Completed                                                       |
| Событие с стандартным типом делегата (без данных в событии) | public event `EventHandler` PascalCase                                                                    | public event `EventHandler` Completed                                                              |
| Событие с стандартным типом делегата (с данными в событии)  | public event `EventHandler<TEventArgs>` PascalCase                                                        | public event `EventHandler<ClickedEventHandler>` Completed                                         |
| Событие с аксессорами `add/remove`                          | public event EventHandler PascalCase { `add` { pascalCase += value; } `remove` { pascalCase -= value; } } | public event EventHandler Completed { add { completed += value; } remove { completed -= value; } } |
| Класс-аргумент для события/делегата                         | public sealed class PascalCaseEventArgs : EventArgs                                                       | public sealed class ClickedEventArgs : EventArgs                                                   |

## Советы по коду

-   Абстрактные / базовые классы называть общим именем без приписок по типу `Base`;
-   У классов указывать ключевое слово `sealed`: `public sealed class PascalCase`, при отсутствии наследования;
-   Неизменяемые данные лучше хранить в типе `record`;
-   Входящие аргументы в конструктор **ВСЕГДА** проверять на `null`, можно через метод: `ArgumentNullException.ThrowIfNull()`;
-   **По возможности** все аргументы конструктора и поля, которым они присваиваются, должны быть интерфейсом;
-   **Поля** лучше инкапсулировать и использовать вместо них свойства.
-   **Простые элементы** иногда лучше оборачивать в класс-обертку. Как пример, вместо `string` использовать `Enum`, так получается более типизировано и проще в поддержке.

[1]: https://docs.microsoft.com/ru-ru/dotnet/standard/design-guidelines/naming-guidelines
[2]: https://github.com/AnatoliyCh/bot-get-by-link/blob/dev/docs/NamingConvention/README.ru.md#папки-и-директории
[3]: https://github.com/AnatoliyCh/bot-get-by-link/blob/dev/docs/NamingConvention/README.ru.md#элементы-языка
[4]: https://github.com/AnatoliyCh/bot-get-by-link/blob/dev/docs/NamingConvention/README.ru.md#советы-по-коду

[← Назад](https://github.com/AnatoliyCh/bot-get-by-link)
