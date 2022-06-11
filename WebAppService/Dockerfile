FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app


ENV PORT=$PORT

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE $PORT
EXPOSE 80
ENTRYPOINT ["dotnet", "WebAppService.dll"]
# With the following line heroku can launch the app
# CMD ASPNETCORE_URLS=http://*:$PORT dotnet WebAppService.dll