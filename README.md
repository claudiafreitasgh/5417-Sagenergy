Sagenergy

Descrição

A Sagenergy é uma aplicação Web desenvolvida para a gestão de serviços de manutenção elétrica e pedidos de assistência de empresas.

A aplicação permite gerir serviços, clientes e pedidos de assistência, incluindo a associação de serviços a cada pedido.

O projeto foi desenvolvido no âmbito da UFCD 5417 – CET 105 no CINEL-Lisboa.

Tecnologias

- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Repository Pattern
- ASP.NET Core Identity
- Bootstrap
- Git / GitHub

Principais Funcionalidades

- Registo de utilizadores
- Login e Logout
- Alteração da palavra-passe
- Recuperação da palavra-passe
- Alteração dos dados do perfil
- Autenticação e autorização
- Gestão de serviços
- Gestão de clientes
- Gestão de pedidos de assistência
- Associação de serviços aos pedidos de assistência
- Validação dos dados
- Controlo de acesso por funções (Admin e Customer)

Modelo de Dados

As principais entidades da aplicação são:

- Service
- Client
- ServiceRequest
- ServiceRequestDetail
- User

Os principais relacionamentos são:

- Um Client pode ter vários ServiceRequests.
- Um ServiceRequest pode conter vários Services através de ServiceRequestDetail.
- Um Service pode estar associado a vários ServiceRequests através de ServiceRequestDetail.
- Um User pode estar associado a um Client.

Estrutura do Projeto

A aplicação segue a arquitetura MVC e utiliza o Repository Pattern.

- Controllers – controladores da aplicação
- Data/Entities – entidades da aplicação
- Data – DbContext e repositories
- Helpers – classes auxiliares da aplicação
- Models – ViewModels
- Views – vistas MVC
- Migrations – migrations do Entity Framework Core

Como Executar

1. Clonar o repositório do GitHub.
2. Abrir a solução no Visual Studio.
3. Garantir que o SQL Server está instalado e em execução.
4. Configurar a ligação à base de dados no ficheiro appsettings.json.
5. Abrir a Package Manager Console no Visual Studio.
6. Executar:

No powershell
Update-Database

7. Compilar e executar a aplicação.

Configuração da Base de Dados

A aplicação utiliza SQL Server e Entity Framework Core.

A ligação à base de dados está configurada no ficheiro:
appsettings.json

A estrutura da base de dados é criada e atualizada através das Migrations do Entity Framework Core.

Administrador
A aplicação cria uma conta de administrador através do processo de Seed da base de dados.
As credenciais do administrador estão configuradas no processo de Seed da aplicação.

GitHub
O código-fonte e o histórico de desenvolvimento do projeto estão disponíveis no repositório GitHub.
