[← Back](https://github.com/AnatoliyCh/bot-get-by-link)

# Introduction

This guide is based on official [recommendations of Microsoft][1]. The `PascalCase` and `camelCase` naming styles are mostly used.  
**git branches** are named using the `kebab-case` naming style.

Navigation:

-   [Folders and directories][2];
-   [Language elements][3];
-   [Code Tips][4];

## Folders and directories

| Object                       | Recommendation                                           | Example                                          |
| ---------------------------- | -------------------------------------------------------- | ------------------------------------------------ |
| Solution                     | MyCompany.MyTechnology                                   | Bot.GetByLink                                    |
| Solution directory           | MyCompany.MyTechnology.TotalName                         | Bot.GetByLink.Common                             |
| Project                      | MyCompany.MyTechnology.FirstFeature                      | Bot.GetByLink.Common.Infrastructure              |
| Project folder               | PascalCase                                               | Abstractions                                     |
| File class / record / struct | PascalCase                                               | ProxyResponseContent.cs                          |
| Interface file               | IPascalCase                                              | IProxyContent.cs                                 |
| Config file `.json`          | appsettings или PascalCase                               | appsettings.json                                 |
| Blocks and fields `.json`    | "PascalCase"                                             | "Clients": { "Telegram": { "Token": "", }, },    |
| Namespaces                   | MyCompany.(Product\|Technology)[.Feature][.subnamespace] | Bot.GetByLink.Common.Infrastructure.Abstractions |

## Language elements

For all **objects** of the language _(class, structure, record, interface and etc.)_ **necessary EXPLICITLY specify the access modifier** for fields, methods/function, properties and etc.  
All `public` elements (fields, methods/function, properties and etc.) mast be declared: `/// <summary> Comment </summary>`.

General sorting of elements:

1. fields;
2. constructor (any access modifier);
3. properties;
4. methods;

#### Language objects

Generic template parameter names are of the form `T` or `TPascalCase`: `public abstract class AsyncCommand<TName>`, `public interface IMessageContext<TChatId, TText, TArtifact>`.  
Covariance (`out T`) and contravariance (`in T`) of generic template parameters are used: `public interface ICommandInvoker<in TCommandName>`.  
Specify the `protected` access modifier for the **abstract class** constructor. If there is at least 1 `abstract` element, the class must be made **abstract**.

| Object          | Recommendation                           | Example                                      |
| --------------- | ---------------------------------------- | -------------------------------------------- |
| Class / struct  | public sealed class PascalCase           | public sealed class UrlRegexWrapper          |
| Extension class | public static class PascalCaseExtensions | public static class LoggingBuilderExtensions |
| Record          | public sealed record PascalCase          | public sealed record Message                 |
| Interface       | IPascalCase                              | public interface IMessageContext             |

#### Fields / properties

Sorting **fields** in a file based on availability: `private readonly`, `private`, `public`, `protected`.  
To resolve a `null-value` use `?`: `private CancellationTokenSource? cts;`.  
**Field / propertie** read-only: `private readonly Type camelCase`, `public Type PascalCase { get; }`.

Tuple unfolding: `var (first, second) = PascalCase()`;

| Object                                    | Recommendation                           | Example                              |
| ----------------------------------------- | ---------------------------------------- | ------------------------------------ |
| Field `private`                           | private Type camelCase                   | private CancellationTokenSource? cts |
| Field `private readonly`                  | private readonly Type camelCase          | private readonly ILogger logger      |
| Field `public`                            | public Type PascalCase                   | public ILogger Logger                |
| Field `protected`                         | protected Type camelCase                 | protected ILogger logger             |
| Propertie `public`                        | public Type PascalCase { get; set; }     | public string Url { get; set; }      |
| Propertie `public` with initialization    | public Type PascalCase { get; init; }    | public string Url { get; init; }     |
| Propertie `public readonly`               | public Type PascalCase { get; }          | public Status State { get; }         |
| Propertie `protected`                     | protected Type PascalCase { get; set; }  | protected string Url { get; set; }   |
| Propertie `protected` with initialization | protected Type PascalCase { get; init; } | protected string Url { get; init; }  |
| Propertie `protected readonly`            | protected Type PascalCase { get; }       | protected Status State { get; }      |

#### Functions / methods

Sorting **functions / methods** in a file based on availability: `public`, `protected`, `private`.  
**Async-methods** should always return `Task<T>` (instead of **void** `Task`), as behaviour can be ambiguous.

Tuple as return type of **methods/functions**: `(Type First, Type Second) PascalCase()`.  
To deconstruct a `Type` into a tuple, use the method `Deconstruct`: `public void Deconstruct(out Type First, out Type Second)`.

| Object                             | Recommendation                                                               | Example                | Return type          |
| ---------------------------------- | ---------------------------------------------------------------------------- | ---------------------- | -------------------- |
| Method / function                  | PascalCase                                                                   | Execute                | object \| null       |
| Method `async/await`               | PascalCaseAsync                                                              | ExecuteAsync           | Task<object \| null> |
| Method с `try/catch`               | TryPascalCase                                                                | TryGetValue            | object \| null       |
| Method `async/await` + `try/catch` | TryPascalCaseAsync                                                           | TryExecuteCommandAsync | Task<object \| null> |
| Method / function argument         | PascalCase(type camelCasing, type camelCasing)                               | IsMatch(string url)    | object \| null       |
| Local variables                    | PascalCase(type camelCasing, type camelCasing) { var/type camelCasing = ...} |                        |                      |

#### Events / delegates

Events / delegates always refer to some action that is happening or has already happened. They should be called using verbs, and the time of the verb (present or past) will indicate the time the event occurred: `Clicked`, `Painting`, `DroppedDown`.  
Events/delegates argument classes named with the suffix `EventArgs` and are usually a type derived from `System.EventArgs` (in new language version **this is optional**): `PascalCaseEventArgs : EventArgs`.  
When **calling** events and delegates, exactly the same syntax for calling methods is used: `object?.Invoke()` or `object?.Invoke(this, PascalCaseEventArgs e)`.

What to choose `delegate` or `event`?

-   `delegate`: if need return type or call an event from outside;
-   `event`: closed call (only the object that contains it) and handling **follow/unfollow** via accessors `add/remove`.

---

The type name `delegate` written with a verb and the suffix `EventHandler`.  
Required delegate parameter are `object sender` (the object that called this delegate), and any return type.

There are several standard types for `delegate`

-   `Action<T...T16>`. `<T...T16>` delegate argumentes. Return `void`;
-   `Func<T1...T16, out TResult>`. `<T...T16>` delegate argumentes. Return `TResult`;
-   `Predicate<T>`. `<T>` single delegate argument. Return `bool`. This type is used **for single value tests**.

---

Events can be declared additionally as `static`, `private`, `virtual`, `abstract`. Event handlers are **called sync**, if there are some followers.  
For `async/await` handler methods, it is better to wrap it in **lambda-async functions** and add `try/catch` (due to `void` return type): `object.Event =+ async (sender, eventArgs) => { await PascalCase() }`.

The `event` type must be a delegate type(`PascalCaseEventHandler` with a`void` return type) or standard types:

-   `EventHandler`. Delegate for all event that do not include event data. When calling, you can use `EventArgs.Empty`. Listener method: `public void OnProgress(object sender, EventArgs e) { ... }`;
-   `EventHandler<TEventArgs>`. For events that include data to send to handlers. Listener method for `EventHandler<bool>`: `public void OnProgress(object sender, bool IsSuccessful) { ... }`;

The name of the event handler method (the method that we subscribe to) usually contains the name of the event with the prefix `On`:
`EventHandler / EventHandler<TEventArgs> / PascalCaseEventHandler OnProgress(object sender, EventArgs / TEventArgs / PascalCaseEventArgs e) { ... }`.

| Object                                                        | Recommendation                                                                                            | Example                                                                                            |
| ------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------- |
| Delegate                                                      | public delegate void PascalCaseEventHandler(object sender, PascalCaseEventArgs e)                         | public delegate void ClickedEventHandler(object sender, ClickedEventArgs count)                    |
| Event with own delegate type                                  | public event `PascalCaseEventHandler` PascalCase                                                          | public event `ClickedEventHandler` Completed                                                       |
| Event with standard delegate type (without data in the event) | public event `EventHandler` PascalCase                                                                    | public event `EventHandler` Completed                                                              |
| Event with standard delegate type (with data in the event)    | public event `EventHandler<TEventArgs>` PascalCase                                                        | public event `EventHandler<ClickedEventHandler>` Completed                                         |
| Event with accessors `add/remove`                             | public event EventHandler PascalCase { `add` { pascalCase += value; } `remove` { pascalCase -= value; } } | public event EventHandler Completed { add { completed += value; } remove { completed -= value; } } |
| Class-argument for event/delegate                             | public sealed class PascalCaseEventArgs : EventArgs                                                       | public sealed class ClickedEventArgs : EventArgs                                                   |

## Code Tips

-   Abstract / base classes are called by a common name without postscripts by type `Base`;
-   Specify the keyword for classes `sealed`: `public sealed class PascalCase`, without of inheritance;
-   Immutable data is best stored in a type `record`;
-   Incoming arguments to the constructor **ALWAYS** check for `null`, you can use the method: `ArgumentNullException.ThrowIfNull()`;
-   **If possible** all constructor arguments and the fields they are assigned to must be an interface;
-   **Fields** are better to encapsulate and use properties instead.
-   **Simple elements** are sometimes best wrapped in a wrapper class. As an example, use `Enum` instead of `string`, it is more typed and easier to maintain.

[1]: https://docs.microsoft.com/ru-ru/dotnet/standard/design-guidelines/naming-guidelines
[2]: https://github.com/AnatoliyCh/bot-get-by-link/blob/dev/docs/NamingConvention/README.en.md#folders-and-directories
[3]: https://github.com/AnatoliyCh/bot-get-by-link/blob/dev/docs/NamingConvention/README.en.md#language-elements
[4]: https://github.com/AnatoliyCh/bot-get-by-link/blob/dev/docs/NamingConvention/README.en.md#code-tips

[← Back](https://github.com/AnatoliyCh/bot-get-by-link)
