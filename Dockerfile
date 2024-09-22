# Используем базовый образ ASP.NET для рабочего этапа
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5001

# Этап сборки проекта
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем только файл проекта и выполняем восстановление пакетов
COPY ["WebApplicationTestTask/WebApplicationTestTask.csproj", "WebApplicationTestTask/"]
RUN dotnet restore "WebApplicationTestTask/WebApplicationTestTask.csproj"

# Копируем все файлы проекта и выполняем сборку
COPY . .
WORKDIR "/src/WebApplicationTestTask"
RUN dotnet build "WebApplicationTestTask.csproj" -c ${BUILD_CONFIGURATION} -o /app/build

# Этап публикации проекта
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WebApplicationTestTask.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish /p:UseAppHost=false

# Финальный этап - создание образа для запуска
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WebApplicationTestTask.dll"]
