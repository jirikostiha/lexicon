#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["code/Lexicon.WebApi/Lexicon.WebApi.csproj", "code/Lexicon.WebApi/"]
COPY ["code/Lexicon.DependencyInjection.Autofac/Lexicon.DependencyInjection.Autofac.csproj", "code/Lexicon.DependencyInjection.Autofac/"]
COPY ["code/Lexicon.SQLite/Lexicon.SQLite.csproj", "code/Lexicon.SQLite/"]
COPY ["code/Lexicon/Lexicon.csproj", "code/Lexicon/"]
COPY ["code/Lexicon.Data/Lexicon.Data.csproj", "code/Lexicon.Data/"]
RUN dotnet restore "code/Lexicon.WebApi/Lexicon.WebApi.csproj"
COPY . .
WORKDIR "/src/code/Lexicon.WebApi"
RUN dotnet build "Lexicon.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lexicon.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lexicon.WebApi.dll"]