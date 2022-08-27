#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Bot.GetByLink.Client.Telegram.WebHook/Bot.GetByLink.Client.Telegram.WebHook.csproj", "src/Bot.GetByLink.Client.Telegram.WebHook/"]
COPY ["src/Bot.GetByLink.Client.Telegram.Common/Bot.GetByLink.Client.Telegram.Common.csproj", "src/Bot.GetByLink.Client.Telegram.Common/"]
COPY ["src/Bot.GetByLink.Common.Infrastructure/Bot.GetByLink.Common.Infrastructure.csproj", "src/Bot.GetByLink.Common.Infrastructure/"]
COPY ["src/Bot.GetByLink.Common.Abstractions/Bot.GetByLink.Common.Abstractions.csproj", "src/Bot.GetByLink.Common.Abstractions/"]
COPY ["src/Bot.GetByLink.Common.Interfaces/Bot.GetByLink.Common.Interfaces.csproj", "src/Bot.GetByLink.Common.Interfaces/"]
COPY ["src/Bot.GetByLink.Common.Enums/Bot.GetByLink.Common.Enums.csproj", "src/Bot.GetByLink.Common.Enums/"]
COPY ["src/Bot.GetByLink.Common.Resources/Bot.GetByLink.Common.Resources.csproj", "src/Bot.GetByLink.Common.Resources/"]
COPY ["src/Bot.GetByLink.Proxy.Reddit/Bot.GetByLink.Proxy.Reddit.csproj", "src/Bot.GetByLink.Proxy.Reddit/"]
COPY ["src/Bot.GetByLink.Proxy.Common/Bot.GetByLink.Proxy.Common.csproj", "src/Bot.GetByLink.Proxy.Common/"]
COPY ["src/Bot.GetByLink.Proxy.Vk/Bot.GetByLink.Proxy.Vk.csproj", "src/Bot.GetByLink.Proxy.Vk/"]
RUN dotnet restore "src/Bot.GetByLink.Client.Telegram.WebHook/Bot.GetByLink.Client.Telegram.WebHook.csproj"
COPY . .
WORKDIR "/src/src/Bot.GetByLink.Client.Telegram.WebHook"
RUN dotnet build "Bot.GetByLink.Client.Telegram.WebHook.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bot.GetByLink.Client.Telegram.WebHook.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bot.GetByLink.Client.Telegram.WebHook.dll"]