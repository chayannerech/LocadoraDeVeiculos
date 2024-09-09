# Locadora De Veiculos 2024

<div align="center">

| <img width="60" src="https://user-images.githubusercontent.com/25181517/121405754-b4f48f80-c95d-11eb-8893-fc325bde617f.png"> | <img width="60" src="https://miro.medium.com/v2/resize:fit:300/0*cdEEkdP1WAuz-Xkb.png"> | <img width="60" src="https://raw.githubusercontent.com/altmann/FluentResults/master/resources/icons/FluentResults-Icon-64.png"> | <img width="60" src="https://rodrigoesilva.wordpress.com/wp-content/uploads/2011/04/sqlserver_sql_server_2008_logo.png"> |
|:---:|:---:|:---:|:---:|
| .NET Core | ASP.NET Core | FluentResults | Microsoft SQL Server |
||
| <img width="60" src="https://www.infoport.es/wp-content/uploads/2023/09/entity-core.png"> | <img width="60" src="https://api.nuget.org/v3-flatcontainer/dapper/2.1.35/icon"> | <img width="60" src="https://www.lambdatest.com/blog/wp-content/uploads/2021/03/MSTest.png"> | <img width="60" src="https://user-images.githubusercontent.com/25181517/184103699-d1b83c07-2d83-4d99-9a1e-83bd89e08117.png"> |
| EF Core | Dapper | MSTest | Selenium |

</div>

## Projeto
**Desenvolvido durante o curso Fullstack da [Academia do Programador](https://www.academiadoprogramador.net) 2024**

### Arquitetura
- DDD
- N-Camadas

### Stack:
- NET 8.0
- ASP.NET MVC
- Microsoft Identity
- Microsoft SQL Server
- Entity Framework Core
- AutoMapper
- Dapper
- FluentResults
- Selenium

### Inclui:
- Testes de Unidade
- Testes de Integração
- Testes e2e
- Autenticação e Autorização com Microsoft Identity
---

## Detalhes

Este projeto é um sistema completo para o gerenciamento das operações de uma locadora de automóveis. A plataforma abrange desde o cadastro de funcionários, clientes, grupos de automóveis e veículos até a configuração de preços, gestão de aluguéis e devoluções, oferecendo uma solução robusta para empresas que desejam otimizar seus processos de locação de veículos.

#### 1. Gestão de Usuários e Empresas
- **Registro de Empresas**: Permite que empresas de locação de veículos se registrem na plataforma e criem contas para seus funcionários.
- **Administração de Usuários**: Usuários administradores podem adicionar, editar, ativar e desativar contas de funcionários.

#### 2. Cadastro e Gerenciamento de Recursos
- **Funcionários e Clientes**: Cadastro detalhado de funcionários e clientes, com informações essenciais para a gestão.
- **Grupos de Automóveis e Veículos**: Registro e possibilidade de categorização de grupos de automóveis (por tipo, marca, modelo, etc.) e gerenciamento de detalhes dos veículos, como quilometragem, tipo de combustível e disponibilidade.

#### 3. Configuração de Preços de Aluguéis e Taxas
- **Definição de Preços de Aluguel**: Configuração de preços baseada em critérios como grupo de veículo, período de aluguel e plano de cobrança (diária, controlada com quilometragem limitada, livre).
- **Taxas e Serviços Adicionais**: Definição de taxas e serviços extras (ex.: seguro, cadeiras de bebê, GPS), com valores fixos ou calculados por dia, que podem ser adicionados ao contrato de aluguel e afetar o preço final.

#### 4. Gestão de Locações e Devoluções
- **Registro de Locações**: Cadastro de novas locações com detalhes sobre o cliente, veículo alugado, plano escolhido, data de retirada e devolução.
- **Devoluções e Aplicação de Multas**: Registro de devoluções de veículos, com cálculo automático de multas por atraso, danos e inclusão de custo de combustível conforme preços configurados.

#### 5. Cálculo Automatizado de Aluguéis
- **Cálculo Dinâmico de Preço**: Cálculo automático do valor total do aluguel, considerando o tipo do veículo, plano de cobrança, taxas adicionais, duração do aluguel e políticas de devolução.

### Benefícios do Sistema

- **Automatização e Eficiência**: Redução de tarefas manuais através da automação de processos, como cálculos de aluguel e aplicação de multas.
- **Controle e Transparência**: Registro centralizado de todas as operações, proporcionando visibilidade completa sobre os processos de aluguel, pagamento e devolução.
- **Flexibilidade e Customização**: Cada locadora pode configurar seus próprios preços e políticas, adaptando a solução às suas necessidades específicas.
- **Experiência do Usuário**: Interface intuitiva e fácil de usar, garantindo uma experiência positiva tanto para funcionários quanto para clientes.

## Regras de Negócio

- Para os itens que possuem dependências, não será possível acessar suas respectivas páginas de cadastro, enquanto não existirem dependências cadastradas.

#### 1. Gestão de Usuários e Empresas
- Não será possível criar usuários com o mesmo login.
- As senhas de acesso deverão conter mais de 6 caracteres.
- O cadastro de um funcionário implica automaticamente na criação de um novo usuário, com login e senha específicos, porém limitado aos acessos pré definidos.
- Todas as atividades realizadas pelo funcionário serão visíveis à sua empresa, assim como o funcionário terá acesso às informações cabíveis às suas funções

#### 2. Cadastro e Gerenciamento de Pessoas
- Não será possível cadastrar pessoas físicas (clientes ou condutores) que possuam o mesmo documento, seja ele CPF, RG ou CNH.
- Não será possível cadastrar pessoas jurídicas sob o mesmo CNPJ.
- Não será possível cadastrar uma CNH vencida.
- Um cliente pode ser registrado como seu próprio condutor caso seja uma pessoa física, utilizando seu CPF para fins de identificação. No entanto, para clientes que são pessoas jurídicas, é obrigatório o cadastro de um condutor associado à empresa, que deve ser identificado por um CPF válido.
- A data de admissão de um funcionário deve condizer com a realidade, assim como o seu salário.
- Não será possível editar ou excluir um cliente/condutor que esteja relacionado à um aluguel ativo (ainda não devolvido).

#### 3. Grupos de Automóveis, Planos de Cobrança e Veículos
- Grupos são identificados pelo nome, enquanto veículos são identificados pela placa, não sendo possível o cadastro de entidades repetidas.
- Os veículos devem ser produzidos entre o ano de 1950 e o ano atual, enquanto sua capacidade deve estar entre 30 e 120L.
- Cada grupo de automóveis deve possuir seu próprio planejamento de cobrança, não sendo possível sobrepor estas informações.
- Não será possível excluir um grupo associado à um plano ou um veículo.
- Não será possível editar ou excluir um veículo/plano/grupo que esteja relacionado à um aluguel ativo.

#### 3. Taxas e Serviços
- As taxas são identificadas pelo nome, não sendo permitidos registros repetidos.
- Taxas cadastradas sob o nome de "Seguro" são automaticamente identificadas e sua cobrança se torna diária, enquanto as demais taxas podem também possuir cobrança fixa.
- Não será possível editar ou excluir uma taxa que esteja relacionada à um aluguel ativo.

#### 4. Configuração de Preços dos Combustíveis
- A configuração é única para cada empresa, não sendo possível excluí-la, apenas editá-la.

#### 5. Aluguéis
- Um aluguel deve possuír um cliente e seu condutor associado, em porte de uma CNH válida.
- O grupo selecionado deve possuir um plano de cobrança pré definido e veículos associados.
- A data de retirada do veículo deve ser superior à data atual, enquanto a data de devolução deve ser superior à data de retirada.
- O plano deve ser selecionado e as taxas desejadas devem ser marcadas.
- Se um seguro for selecionado, deve-se obrigatoriamente informar a cobertura (do cliente ou de terceiros).
- Ao devolver o aluguel, a data deve ser informada, enquanto o sistema automaticamente calcula e identifica a necessidade de aplicação de multas.
- A quilometragem final deve ser superior à inicial. Seu valor respectivo ao plano selecionado será somado ao valor total.
- Se o tanque for devolvido vazio, será cobrado o valor do combustível.
- Não será possível excluir um aluguel ativo, ou editar um aluguel finalizado.

- .NET SDK (recomendado .NET 8.0 ou superior) para compilação e execução do projeto.
---
## Como Usar

#### Clone o Repositório
```
git clone https://github.com/academia-do-programador/locadora-de-veiculos-2024.git
```

#### Navegue até a pasta raiz da solução
```
cd locadora-de-veiculos-2024
```

#### Restaure as dependências
```
dotnet restore
```

#### Navegue até a pasta do projeto
```
cd LocadoraDeVeiculos.WebApp
```

#### Execute o projeto
```
dotnet run
```
