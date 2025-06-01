# Floodless MVC

Uma aplicação ASP.NET Core MVC desenvolvida para auxiliar na gestão e coordenação de recursos e voluntários durante situações de enchentes. O sistema facilita a organização de doações e a mobilização de voluntários, permitindo uma resposta mais eficiente em momentos de crise.

### Sobre o Projeto
O Floodless é uma solução criada para endereçar um dos principais desafios durante enchentes: a coordenação eficiente entre doações, recursos disponíveis e pessoas dispostas a ajudar. A aplicação serve como uma ponte entre doadores, voluntários e as necessidades das comunidades afetadas.

### Objetivo
- Centralizar informações sobre recursos disponíveis
- Facilitar a coordenação de voluntários
- Otimizar a distribuição de doações
- Manter um histórico organizado de todas as ações
- Permitir uma resposta rápida em situações de emergência

### Principais Benefícios
- **Organização**: Sistema centralizado para gestão de recursos e voluntários
- **Eficiência**: Acompanhamento em tempo real de doações e necessidades
- **Transparência**: Registro claro de todas as movimentações e responsáveis
- **Agilidade**: Rápida identificação de recursos disponíveis e voluntários próximos
- **Controle**: Gestão eficiente do estoque de doações e disponibilidade de voluntários

### Impacto Social
- **Comunidade**: Fortalecimento da rede de apoio local
- **Emergência**: Resposta mais rápida em situações críticas
- **Recursos**: Melhor aproveitamento das doações recebidas
- **Voluntariado**: Incentivo à participação da comunidade
- **Dados**: Geração de informações para melhorar a prevenção

## 📋 Pré-requisitos

- Visual Studio Code
- .NET SDK 8.0
- Docker Desktop
- Azure CLI (para deploy na Azure)
- Git

## 📱 Funcionalidades da Aplicação

### Gestão de Voluntários
- **Cadastro de Voluntários**: Permite registrar pessoas dispostas a ajudar em situações de enchentes
  - Nome completo
  - E-mail para contato
  - Número de telefone
  - Data de registro automática
- **Gerenciamento de Voluntários**:
  - Listagem de todos os voluntários cadastrados
  - Edição de informações
  - Remoção de cadastros
  - Visualização detalhada de cada voluntário

### Gestão de Recursos
- **Cadastro de Recursos**: Registro de itens disponíveis para ajuda humanitária
  - Nome do recurso
  - Tipo do recurso (Alimento, Remédio ou Roupa)
  - Quantidade disponível
  - Data de cadastro automática
  - Vinculação com voluntário responsável
- **Controle de Estoque**:
  - Acompanhamento de quantidades
  - Histórico de doações
  - Relatórios de disponibilidade

### Estrutura do Projeto
- **Camada de Apresentação (MVC)**:
  - `Controllers/`: Controladores para gerenciar as requisições
  - `Views/`: Interface do usuário em Razor
  - `Models/`: Classes de modelo para formulários

- **Camada de Aplicação**:
  - `Application/Services/`: Lógica de negócios
  - `Application/Interfaces/`: Contratos de serviço
  - `Application/DTOs/`: Objetos de transferência de dados

- **Camada de Domínio**:
  - `Domain/Entities/`: Entidades principais (Voluntario e Recurso)
  - `Domain/Enums/`: Enumerações (TipoRecurso)
  - `Domain/Interfaces/`: Contratos de repositório

- **Camada de Infraestrutura**:
  - `Infrastructure/Data/`: Contexto e configurações do Entity Framework
  - `Infrastructure/Repositories/`: Implementação dos repositórios
  - `Infrastructure/Migrations/`: Scripts de banco de dados

### Banco de Dados
- **Tabelas**:
  - `TB_FLOODLESS_VOLUNTARIO`: Armazena dados dos voluntários
  - `TB_FLOODLESS_RECURSO`: Registra recursos disponíveis
- **Relacionamentos**:
  - Um voluntário pode ter vários recursos
  - Cada recurso pertence a um voluntário


## 🚀 Configuração do Ambiente

### Configuração Local (VS Code)

1. Clone o repositório:
```bash
git clone https://github.com/fiap-code-genius/Floodless_MVC.git
cd Floodless_MVC
```

2. Abra o projeto no VS Code:
```bash
code .
```

3. Restaure as dependências:
```bash
dotnet restore
```

4. Execute o projeto:
```bash
dotnet run --project Floodless_MVC
```

A aplicação estará disponível em `http://localhost:5207`

## 🌐 Configuração na VM Azure

### Script de Deployment Azure

```bash
#!/bin/bash

# Variáveis
RESOURCE_GROUP="rg-floodless"
LOCATION="brazilsouth"
VM_NAME="vm-floodless"
IMAGE="almalinux:almalinux-x86_64:9-gen2:9.5.202411260"
SIZE="Standard_D2s_v3"
ADMIN_USERNAME="sunauezuri"
ADMIN_PASSWORD="Fiap@2tdsvms"
DISK_SKU="StandardSSD_LRS"

# Criar grupo de recursos
echo "Criando grupo de recursos: $RESOURCE_GROUP..."
az group create --name $RESOURCE_GROUP --location $LOCATION

# Criar a VM
echo "Criando a máquina virtual: $VM_NAME, por favor aguarde..."
az vm create \
  --resource-group $RESOURCE_GROUP \
  --name $VM_NAME \
  --image $IMAGE \
  --size $SIZE \
  --authentication-type password \
  --admin-username $ADMIN_USERNAME \
  --admin-password $ADMIN_PASSWORD \
  --storage-sku $DISK_SKU \
  --public-ip-sku Basic

echo "Abrindo porta 1521 para o banco Oracle..."
az vm open-port --port 1521 --resource-group $RESOURCE_GROUP --name $VM_NAME --priority 1002

echo "Abrindo porta 8080 para aplicação..."
az vm open-port --port 8080 --resource-group $RESOURCE_GROUP --name $VM_NAME --priority 1003

echo "Abrindo porta 5500 para o Enterprise Manager Express..."
az vm open-port --port 5500 --resource-group $RESOURCE_GROUP --name $VM_NAME --priority 1004

echo "VM Criada com sucesso!"
```

### Modificações Necessárias para Docker

No arquivo `Program.cs`, você precisa:

1. Comentar o trecho de conexão local:
```csharp
//Utilizando Local(Comentar ou apagar esse trecho caso use com Docker)
/*
builder.Services.AddDbContext<ApplicationContext>(x =>
{
    x.UseOracle(builder.Configuration.GetConnectionString("OracleLocal"));
});
*/
```

2. Descomentar o trecho para Docker:
```csharp
//Descomentar esse trecho para usar com Docker 

var config = builder.Configuration;

// Tente obter as variáveis do ambiente
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

string connectionString;

if (!string.IsNullOrEmpty(dbHost) && !string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPassword))
{
    // Ambiente Docker: substituir placeholders
    var connTemplate = config.GetConnectionString("OracleDocker");
    connectionString = connTemplate
        .Replace("${DB_HOST}", dbHost)
        .Replace("${DB_USER}", dbUser)
        .Replace("${DB_PASSWORD}", dbPassword);
}
else
{
    // Ambiente local: usa string fixa
    connectionString = config.GetConnectionString("OracleLocal");
}

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseOracle(connectionString));
```

3. Descomentar essa sessão para criar automaticamente as migrations:

```csharp
// Aplicar migrations automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    db.Database.Migrate();
}
```


### Acessando e Configurando a VM

1. Conecte-se à VM usando SSH:
```bash
ssh sunauezuri@<vm-ip>
```

2. Atualize o sistema:
```bash
sudo dnf update -y
```

3. Instale o Git:
```bash
sudo dnf install git -y
```

4. Instale o Docker:
```bash
sudo dnf install docker
```

5. Verifique as instalações:
```bash
git --version
docker --version
```

6. Clone o repositório:
```bash
git clone https://github.com/fiap-code-genius/Floodless_MVC.git
```

## 🐳 Configuração com Docker

### Configuração da Rede e Containers

1. Configuração da Rede Docker:
```bash
docker network create floodless-network
```

2. Configuração do Banco Oracle:
```bash
docker run -d \
--name oracle-db \
--network floodless-network \
-p 1521:1521 \
-e ORACLE_PASSWORD=fiap24 \
-e ORACLE_ALLOW_REMOTE=true \
-e ORACLE_DISABLE_ASYNCH_IO=true \
-e ORACLE_EDITION=xe \
-v oracle_data:/opt/oracle/oradata \
docker.io/gvenzl/oracle-xe:latest
```

3. Build da Aplicação:
```bash
cd Floodless_MVC
docker build -t floodless-app .
```

4. Execução da Aplicação:
```bash
cd ..
docker run -d --name floodless-app --network floodless-network -p 8080:8080 floodless-app
```

### Acessando a Aplicação na VM

1. Após a execução dos containers, a aplicação estará disponível em:
   - Aplicação Web: `http://<vm-ip>:8080`
   - Banco Oracle: `<vm-ip>:1521`
   - Enterprise Manager Express: `http://<vm-ip>:5500/em`

2. Credenciais padrão:
   - Usuário Oracle: system
   - Senha Oracle: fiap24

## 📦 Estrutura do Dockerfile

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copia todo o código fonte
COPY . .

# Restaura dependências
RUN dotnet restore

# Publica em Release
RUN dotnet publish Floodless_MVC/Floodless_MVC.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

# Cria um usuário não root
RUN adduser -D floodlessuser

# Define variáveis de ambiente
ENV ASPNETCORE_URLS=http://+:8080
ENV DB_HOST=oracle-db
ENV DB_USER=system
ENV DB_PASSWORD=fiap24

# Copia os arquivos publicados do build
COPY --from=build /app/publish ./

# Copia arquivos de configuração
COPY Floodless_MVC/appsettings.json ./appsettings.json
COPY Floodless_MVC/appsettings.Development.json ./appsettings.Development.json

# Altera propriedade dos arquivos para o novo usuário
RUN chown -R floodlessuser:floodlessuser /app

# Troca para o usuário não root
USER floodlessuser

# Expõe a porta usada pelo Kestrel
EXPOSE 8080

# Entry point da aplicação
ENTRYPOINT ["dotnet", "Floodless_MVC.dll"]
```

### Explicação do Dockerfile

O Dockerfile está estruturado em duas etapas principais (multi-stage build) para otimizar o tamanho final da imagem:

#### 1. Etapa de Build
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
```
- Usa a imagem oficial do SDK .NET 8.0 para compilar a aplicação
- `AS build` define um nome para esta etapa que será referenciado posteriormente

```dockerfile
WORKDIR /source
COPY . .
```
- Define o diretório de trabalho como `/source`
- Copia todos os arquivos do projeto para este diretório

```dockerfile
RUN dotnet restore
RUN dotnet publish Floodless_MVC/Floodless_MVC.csproj -c Release -o /app/publish
```
- Restaura todas as dependências do projeto
- Compila e publica a aplicação em modo Release no diretório `/app/publish`

#### 2. Etapa de Runtime
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
```
- Usa a imagem mais leve do ASP.NET Core Runtime baseada em Alpine Linux
- Esta imagem contém apenas o necessário para executar a aplicação

```dockerfile
WORKDIR /app
RUN adduser -D floodlessuser
```
- Define o diretório de trabalho como `/app`
- Cria um usuário não-root por questões de segurança

```dockerfile
ENV ASPNETCORE_URLS=http://+:8080
ENV DB_HOST=oracle-db
ENV DB_USER=system
ENV DB_PASSWORD=fiap24
```
- Define variáveis de ambiente essenciais:
  - `ASPNETCORE_URLS`: Define a porta de escuta da aplicação
  - `DB_HOST`: Nome do container do banco Oracle na rede Docker
  - `DB_USER`: Usuário do banco de dados
  - `DB_PASSWORD`: Senha do banco de dados

```dockerfile
COPY --from=build /app/publish ./
```
- Copia apenas os arquivos publicados da etapa de build
- Isso mantém a imagem final menor, sem incluir código fonte e ferramentas de desenvolvimento

```dockerfile
COPY Floodless_MVC/appsettings.json ./appsettings.json
COPY Floodless_MVC/appsettings.Development.json ./appsettings.Development.json
```
- Copia os arquivos de configuração necessários para a aplicação
- Mantém as configurações separadas do código compilado

```dockerfile
RUN chown -R floodlessuser:floodlessuser /app
USER floodlessuser
```
- Altera o proprietário de todos os arquivos para o usuário não-root
- Muda para o usuário não-root para execução mais segura

```dockerfile
EXPOSE 8080
```
- Documenta que a aplicação escuta na porta 8080
- Não abre a porta automaticamente, apenas serve como documentação

```dockerfile
ENTRYPOINT ["dotnet", "Floodless_MVC.dll"]
```
- Define o comando que será executado quando o container iniciar
- Inicia a aplicação .NET usando o runtime do ASP.NET Core

#### Benefícios desta Estrutura
1. **Segurança**: Utiliza usuário não-root e imagem Alpine Linux minimal
2. **Tamanho**: Multi-stage build reduz significativamente o tamanho da imagem final
3. **Isolamento**: Configurações via variáveis de ambiente permitem flexibilidade
4. **Performance**: Usa imagem base otimizada para ASP.NET Core
5. **Manutenibilidade**: Estrutura clara e bem documentada

## 📊 Diagramas da Solução

### Modelo de Domínio
```
+------------------------+        +------------------------+
|      Voluntario        |        |        Recurso        |
|------------------------|        |------------------------|
| Id: int               |        | Id: int               |
| Nome: string          |        | Nome: string          |
| Email: string         |<-------| TipoRecurso: enum     |
| Contato: string       |        | Quantidade: int       |
| DataRegistro: DateTime |        | DataCriacao: DateTime |
|                       |        | VoluntarioId: int     |
+------------------------+        +------------------------+
                                          |
                                          |
                                 +------------------+
                                 |   TipoRecurso    |
                                 |------------------|
                                 | Alimento        |
                                 | Remedio         |
                                 | Roupa           |
                                 +------------------+
```

### Arquitetura em Camadas
```
+-------------------+     +------------------+     +------------------+
|   Controllers     |     |    Services     |     |    Entities     |
| VoluntarioCtrl   |---->| VoluntarioApp   |---->| Voluntario      |
| RecursoCtrl      |     | RecursoApp      |     | Recurso         |
+-------------------+     +------------------+     +------------------+
          |                       |                        |
          |                       |                        |
          v                       v                        v
+------------------+     +------------------+     +------------------+
| Repositories     |     |    Database      |     |      DTOs       |
| VoluntarioRepo   |<--->|  Oracle Tables   |     | VoluntarioDto  |
| RecursoRepo      |     |                  |     | RecursoDto     |
+------------------+     +------------------+     +------------------+
```

### Arquitetura Docker
```
+------------------+     +------------------+
|                  |     |                  |
|  Floodless App   |<--->|   Oracle DB     |
|  Container       |     |   Container      |
|  (ASP.NET Core)  |     |                 |
+------------------+     +------------------+
         |                       |
         |                       |
         v                       v
+------------------------------------------+
|                                          |
|           Docker Network                 |
|         (floodless-network)              |
|                                          |
+------------------------------------------+
```

## 🧪 Exemplos de Testes

### Criação


![Captura de tela 2025-05-31 224701](https://github.com/user-attachments/assets/11f73cb9-ed79-44ab-9201-04ad4b4bd5c9)

![Captura de tela 2025-05-31 224716](https://github.com/user-attachments/assets/314f7177-8301-4e7c-bc30-901b76a240aa)

![Captura de tela 2025-05-31 224727](https://github.com/user-attachments/assets/aee67f45-492c-4bb9-a41c-8f799aaf05f9)


### Detalhes


![image](https://github.com/user-attachments/assets/7f6ab0f8-9e09-4c1e-bd64-fe948ac8b575)


### Editar

![Captura de tela 2025-05-31 224745](https://github.com/user-attachments/assets/f58303f3-f10e-44bd-b5c8-22ad0b6f32cd)

![Captura de tela 2025-05-31 224800](https://github.com/user-attachments/assets/c7033567-d932-47d4-b68b-3e6d1825e2a2)


# Deletar

![Captura de tela 2025-05-31 224810](https://github.com/user-attachments/assets/6b642a3b-8228-4cb1-9524-e1cca3e9c23a)

![Captura de tela 2025-05-31 224823](https://github.com/user-attachments/assets/58588953-3d4f-46bb-aed1-161455b0ea70)


# Links

 - Apresentação do projeto: https://youtu.be/gCQDYh8cYfY
 - Aplicação em cloud com 2 Containers: https://youtu.be/yr3VW_Mnbsk





