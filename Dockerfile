FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

EXPOSE 8444

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./Server/Server.csproj", "./"]
COPY ["./Domain/", "../Domain/"]
COPY ["./Logic/", "../Logic/"]
RUN dotnet restore
COPY . .

WORKDIR /src/Domain
RUN dotnet build -c Release -o /app

WORKDIR /src/Logic
RUN dotnet build -c Release -o /app

WORKDIR /src/Server
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install -g dotnet-ef --version 7.0.7

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=publish /src/Logic/command.json ../Logic/command.json

CMD ["dotnet", "Server.dll"]