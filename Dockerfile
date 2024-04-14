FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-env
WORKDIR /App


RUN git clone https://github.com/Maxime-Pages/RanceConnect.git
WORKDIR /App/RanceConnect
RUN git switch server

# Copy everything
#COPY . ./
# Restore as distinct layers
RUN dotnet restore	./srv/RanceServer.csproj
# Build and publish a release
RUN dotnet publish ./srv/RanceServer.csproj -c Release -o ./built

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/RanceConnect/built .
ENTRYPOINT ["./RanceServer"]
