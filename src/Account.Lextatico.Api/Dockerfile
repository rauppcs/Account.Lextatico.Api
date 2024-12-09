FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 9002
EXPOSE 9003

ENV ASPNETCORE_URLS=http://+:9002;https://+:9003

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
# RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
# USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Account.Lextatico.Api/Account.Lextatico.Api.csproj", "src/Account.Lextatico.Api/"]
RUN dotnet restore "src/Account.Lextatico.Api/Account.Lextatico.Api.csproj"
COPY . .
WORKDIR "/src/src/Account.Lextatico.Api"
RUN dotnet build "Account.Lextatico.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Account.Lextatico.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Account.Lextatico.Api.dll"]
