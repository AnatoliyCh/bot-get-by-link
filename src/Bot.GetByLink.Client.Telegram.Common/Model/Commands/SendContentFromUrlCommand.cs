﻿using Bot.GetByLink.Client.Telegram.Common.Enums;
using Bot.GetByLink.Client.Telegram.Common.Interfaces;
using Bot.GetByLink.Client.Telegram.Common.Model.Exceptions;
using Bot.GetByLink.Client.Telegram.Common.Model.Regexs;
using Bot.GetByLink.Common.Abstractions.Command;
using Bot.GetByLink.Common.Enums;
using Bot.GetByLink.Common.Interfaces;
using Bot.GetByLink.Common.Interfaces.Command;
using Bot.GetByLink.Common.Interfaces.Proxy;
using Bot.GetByLink.Common.Resources;
using Telegram.Bot.Types;

namespace Bot.GetByLink.Client.Telegram.Common.Model.Commands;

/// <summary>
///     Returns content post Reddit.
/// </summary>
public sealed class SendContentFromUrlCommand : AsyncCommand<CommandName>
{
    private readonly IBuilderMessage builderMessage;
    private readonly IAsyncCommand<CommandName> sendMessageCommand;
    private readonly IRegexWrapper urlRegex;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SendContentFromUrlCommand" /> class.
    /// </summary>
    /// <param name="sendMessageCommand">Sends a message to the client.</param>
    /// <param name="proxyServices">Proxy collection.</param>
    /// <param name="regexWrappers">Regular expressions for checks.</param>
    /// <param name="builderMessage">Builder message.</param>
    public SendContentFromUrlCommand(
        IAsyncCommand<CommandName> sendMessageCommand,
        IEnumerable<IProxyService> proxyServices,
        IEnumerable<IRegexWrapper> regexWrappers,
        IBuilderMessage builderMessage)
        : base(CommandName.SendContentFromUrl)
    {
        this.sendMessageCommand = sendMessageCommand ?? throw new ArgumentNullException(nameof(sendMessageCommand));
        this.builderMessage = builderMessage;
        ProxyServices = proxyServices ?? throw new ArgumentNullException(nameof(proxyServices));
        urlRegex = GetUrlRegexWrapperByIRegexWrappers(regexWrappers) ??
                   throw new ArgumentNullException(nameof(regexWrappers));
    }

    /// <summary>
    ///     Gets a collection of proxy services.
    /// </summary>
    public IEnumerable<IProxyService> ProxyServices { get; }

    /// <summary>
    ///     Collect and send content post.
    /// </summary>
    /// <param name="ctx">Update client.</param>
    /// <returns>Empty Task.</returns>
    public override async Task ExecuteAsync(object? ctx)
    {
        if (ctx is not Update update) throw new ClientException(ExceptionType.Technical, "ctx is not Update");

        var chatId = update.Message?.Chat.Id;
        var text = update.Message?.Text;
        if (chatId is null || string.IsNullOrWhiteSpace(text))
        {
            var messageException = string.Format("Command: {0} => chatId: {1}, text: {2}", Name, chatId, text);
            throw new ClientException(ExceptionType.Technical, messageException);
        }

        var url = urlRegex.Match(text)?.Value;
        if (string.IsNullOrWhiteSpace(url)) throw new ClientException();

        var matchProxy = ProxyServices.FirstOrDefault(proxy => proxy.IsMatch(url));
        if (matchProxy is null) throw new ClientException();

        var postContent = await matchProxy.GetContentUrlAsync(url);
        if (postContent is null)
        {
            var messageException = ResourceRepository.GetClientResource(ClientResource.FailedGetResource);
            throw new ClientException(ExceptionType.Allowed, messageException, chatId);
        }

        var message = builderMessage
            .From(postContent)
            .AddUrl(url)
            .AddChatId(chatId ?? -1)
            .SetHeaders()
            .Build();

        await sendMessageCommand.ExecuteAsync(message);
    }

    private static UrlRegexWrapper? GetUrlRegexWrapperByIRegexWrappers(IEnumerable<IRegexWrapper>? regexWrappers)
    {
        if (regexWrappers is null || !regexWrappers.Any()) return null;

        foreach (var regexWrapper in regexWrappers)
            if (regexWrapper is UrlRegexWrapper regex)
                return regex;

        return null;
    }
}