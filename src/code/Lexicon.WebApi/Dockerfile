FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /appproject
COPY . .
RUN ls -la
WORKDIR /appproject/src/code/Lexicon.WebApi

#RUN ls -la
RUN dotnet build "Lexicon.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lexicon.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lexicon.WebApi.dll"]