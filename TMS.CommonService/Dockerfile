FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TMS.CommonService/TMS.CommonService.csproj", "TMS.CommonService/"]
COPY ["TMS.Lib/TMS.Lib.csproj", "TMS.Lib/"]
RUN dotnet restore "TMS.CommonService/TMS.CommonService.csproj"
COPY . .
WORKDIR "/src/TMS.CommonService"
RUN dotnet build "TMS.CommonService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TMS.CommonService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TMS.CommonService.dll"]