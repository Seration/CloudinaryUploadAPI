FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["CloudinaryMediaUpload.API/CloudinaryMediaUpload.API.csproj", "CloudinaryMediaUpload.API/"]
RUN dotnet restore "CloudinaryMediaUpload.API/CloudinaryMediaUpload.API.csproj"
COPY . .
WORKDIR "/src/CloudinaryMediaUpload.API"
RUN dotnet build "CloudinaryMediaUpload.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CloudinaryMediaUpload.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CloudinaryMediaUpload.API.dll"]