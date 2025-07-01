# Finance Control

Sistema de controle financeiro pessoal desenvolvido em ASP.NET Core Razor Pages (.NET 9).

## Estrutura do Projeto

- **Web**: Interface web (Razor Pages)
- **Application**: Lógica de aplicação e serviços
- **Domain**: Entidades de domínio e regras de negócio
- **Infrastructure**: Persistência de dados e integrações
- **SharedKernel**: Componentes e contratos compartilhados
- **Tests**: Testes automatizados

## Funcionalidades Principais

- **Dashboard**: Visão geral de receitas, despesas, saldo e gráficos mensais
- **Transações**: Cadastro, filtro, paginação e gerenciamento de receitas/despesas
- **Categorias**: Organização das transações por categoria
- **Metas**: Definição e acompanhamento de metas financeiras
- **Parcelas**: Controle de pagamentos parcelados

## Requisitos

- .NET 9 SDK
- SQLite (padrão) ou outro banco configurado em `appsettings.json`

## Como Executar

1. Clone o repositório
2. Restaure os pacotes NuGet:dotnet restore3. Execute as migrations para criar/atualizar o banco de dados:dotnet ef database update --project Web4. Rode o projeto Web:dotnet run --project Web5. Acesse em `http://localhost:5000` (ou porta configurada)

## Testes

Execute os testes automatizados com:dotnet test
## Contribuição

Pull requests são bem-vindos! Para contribuir:
- Crie uma branch a partir da `main`
- Faça suas alterações e envie um PR

## Licença

Este projeto está sob a licença MIT.
