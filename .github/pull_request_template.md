### Проверить / Checks:

- [ ] комментарии для документации с большой буквы | capitalized documentation comments;
- [ ] `private set` / отсутcтвие `set` по возможности заменить на `init` | `privat set` / absence of `set`, if possible, replace with `init`;
- [ ] добавить модификатор `sealed` к классам без наследников | add the `sealed` modifier to classes without descendants;
- [ ] убрать кавычки `{}` с `namespace` | remove quotes `{}` from `namespace`;
- [ ] все регулярные выражения обернуть в `IRegexWrapper` | wrap all regular expressions in `IRegexWrapper`;
- [ ] добавить ссылки на [issues](https://github.com/AnatoliyCh/bot-get-by-link/issues) | add links to [issues](https://github.com/AnatoliyCh/bot-get-by-link/issues);
- [ ] убрать лейбл **in progress** с закрепленных [issue](https://github.com/AnatoliyCh/bot-get-by-link/issues?q=is%3Aissue+is%3Aopen+label%3A%22in+progress%22) | remove **in progress** label from pinned [issue](https://github.com/AnatoliyCh/bot-get-by-link/issues?q=is%3Aissue+is%3Aopen+label%3A%22in+progress%22);
- [ ] выполнить команду `dotnet jb cleanupcode Bot.GetByLink.sln` | run command `dotnet jb cleanupcode Bot.GetByLink.sln`;
- [ ] соединить коммиты (squash) и дать адекватные названия | squash commits and give adequate names.
