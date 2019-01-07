FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy everything and build
COPY ./ ./
RUN dotnet publish -r ubuntu-x64 -c Release
# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/bin/Release/netcoreapp2.1/ubuntu-x64/publish/ .
CMD dotnet IdentityServer.dll