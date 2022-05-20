<h1 align="center">Back-End Criação do usuário</h1>

## Descrição do Projeto

<p align="center">Desenvolver o backend utilizando como base as informações enviadas do frontend, informações essas providas do envio de um formulário.
</p>

## Contexto
Aqui na Warren seguimos o padrão de arquitetura Onion Architecture, esse desafio tem como objetivo implementar uma arquitetura de software como continuação do desafio anterior separando responsabilidades em camadas além de desacoplar serviços da api.

## Documentação técnica do desafio

### Requisições HTTP:
Criaremos 3 camadas sendo elas respectivamente:

- [x] Camada de Domínio(DomainServices);

- [x] Camada de aplicação(AppServices);

- [x] Camada de entidades de domínio(DomainModels).

Classe de banco de dados passará para camada de domínio(DomainServices).
Criaremos uma classe(AppService) que será chamada pela api e redirecionará os dados recebidos para camada de domínio(DomainServices).

### Pontos de atenção
Camada de api se comunica apenas com camada de aplicação(AppServices), camada de aplicação(AppServices) se comunica com camada de domínio(DomainServices) e outras AppServices.

- [x] Classe de banco de dados passará para camada de domínio(DomainServices);

- [x] CustomerService será equivalente a uma DomainService;

- [x] Criaremos uma classe(AppService) que será chamada pela api e redirecionará os dados recebidos para camada de domínio(Banco de dados).

### Critérios para aceitação

- [x] Validações devem pertencer a camada de aplicação;

- [x] Única responsabilidade da api é receber requisições, endereçar requisições a camada de aplicação e retornar status code/response de acordo com o retorno da camada de aplicação.

## Pré-requisitos

Antes de começar, você vai precisar ter instalado em sua máquina as seguintes ferramentas:
[Git](https://git-scm.com), [.NET](https://dotnet.microsoft.com/en-us/download). 
Além disto é bom ter um editor para trabalhar com o código como [Visual Studio Code](https://code.visualstudio.com/download)

## 🎲 Rodando o Back End (servidor)

```bash
# Clone este repositório
$ https://github.com/santosclaudinei-warren/Desafio_OnionArchitecture

# Acesse a pasta do projeto no terminal/cmd
$ cd Desafio_OnionArchitecture/src/AppServices

# Execute a aplicação
$ Executar o comando dotnet run 

# O servidor inciará na porta:4231 
- Utilizando a URL <http://localhost:5160>
```

## 🛠 Tecnologias

As seguintes ferramentas foram usadas na construção do projeto:

- [.NET](https://dotnet.microsoft.com/en-us/)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [Postman](https://www.postman.com/downloads/)

## Licença

Licensed under the [MIT license](LICENSE).

## Autor

<b>Claudinei Santos</b>

Encontrei dificuldades em:

- [x] Entender a estrutura Onion Architecture;
- [x] Criar as Class Library através do CLI do .NET;
- [x] Referenciar as camadas em outra camada;

Consegui compreender um pouco mais da arquitetura depois de superar as dificuldades acima citadas.
Em meio a minhas horas de estudo pude aprender algumas coisas que eu gostei e acabei implementando no desafio. Como por exemplo:

- [x] Uma Entidade Base;
- [x] Data de Criação do Customer;
- [x] Data de Atualização do Customer;
- [x] Criar um Modulo para cadastrar meus services que fazem injeções de dependencias;

[![Linkedin Badge](https://img.shields.io/badge/-Claudinei-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/claudinei-santos-ti/)](https://www.linkedin.com/in/claudinei-santos-ti/)
[![Gmail Badge](https://img.shields.io/badge/-santos.devclaudinei@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:santos.devclaudinei@gmail.com)](mailto:claudinei.santos@warren.com.br)