#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["app.Web/app.Web.csproj", "app.Web/"]
COPY ["app.Data/app.Data.csproj", "app.Data/"]
COPY ["app.Domain/app.Domain.csproj", "app.Domain/"]
COPY ["app.Service/app.Service.csproj", "app.Service/"]
RUN dotnet restore "app.Web/app.Web.csproj"
COPY . .
WORKDIR "/src/app.Web"
RUN dotnet build "app.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "app.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "app.Web.dll"]