# Stage: Development

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS development

RUN apt-get update -y
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash --debug
RUN apt-get install nodejs -yq

COPY . /src/dotnet-function-app
RUN cd /src/dotnet-function-app && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish src/Wiz.Template.Function/*.csproj --output /home/site/wwwroot

# To enable ssh & remote debugging on app service change the base image to the one below
# FROM mcr.microsoft.com/azure-functions/dotnet:3.0-appservice
FROM mcr.microsoft.com/azure-functions/dotnet:3.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ViaCEPUrl=https://viacep.com.br/ws/
ENV Issuer=URL_SSO
ENV Audience=SSO_SCOPE
ENV AzureWebJobsStorage=URL_STORAGE

EXPOSE 7071

COPY --from=development ["/home/site/wwwroot", "/home/site/wwwroot"]