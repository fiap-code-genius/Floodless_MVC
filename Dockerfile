# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copia todo o código fonte
COPY . .

# Restaura dependências
RUN dotnet restore "Floodless_MVC.csproj"

# Publica em Release
RUN dotnet publish "Floodless_MVC.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

# Cria um usuário não root
RUN adduser -D floodlessuser

# Define variáveis de ambiente
ENV ASPNETCORE_URLS=http://+:8080
ENV DB_HOST=oracle-db
ENV DB_USER=system
ENV DB_PASSWORD=oracle

# Copia os arquivos publicados do build
COPY --from=build /app/publish ./

# Copia arquivos de configuração
COPY appsettings.json ./appsettings.json
COPY appsettings.Development.json ./appsettings.Development.json

# Altera propriedade dos arquivos para o novo usuário
RUN chown -R floodlessuser:floodlessuser /app

# Troca para o usuário não root
USER floodlessuser

# Expõe a porta usada pelo Kestrel
EXPOSE 8080

# Entry point da aplicação
ENTRYPOINT ["dotnet", "Floodless_MVC.dll"] 