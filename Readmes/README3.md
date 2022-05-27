## Contexto
Seguindo boas práticas de construções de api, neste desafio definiremos DTOs para nossas entidades de domínio(Customer).

## Documentação técnica do desafio

### Será utilizado a biblioteca AutoMapper para mapeamento de entidades.

* Profiles de mapeamento das entidades definidos na camada de aplicação.

### Critérios para aceitação

* Requests recebidas pelos endpoints da api devem ser mapeadas para entidades de domínio; Ex: GetCustomerByDocumentRequest

* Antes das AppServices retornarem os dados para api devemos mapear a(s) entidade(s) de domínio para entidade(s) de response da api; Ex: CustomerResult

* Todos os mapeamentos de entidade(s) devem serem feitas na camada de aplicação(Application)

## Pré-requisitos

Antes de começar, você vai precisar ter instalado em sua máquina as seguintes ferramentas:
[Git](https://git-scm.com), [.NET](https://dotnet.microsoft.com/en-us/download). 
Além disto é bom ter um editor para trabalhar com o código como [Visual Studio Code](https://code.visualstudio.com/download)

## 🎲 Rodando o Back End (servidor)

```bash
# Clone este repositório
$ https://github.com/santosclaudinei-warren/Desafio_OnionArchitecture

# Acesse a pasta do projeto no terminal/cmd
$ cd Desafio_AspNetWebAPI/src/API

# Execute a aplicação
$ Executar o comando dotnet run 

# O servidor inciará na porta:5160 
- Utilizando a URL <http://localhost:5160>
```

## 🛠 Tecnologias

As seguintes ferramentas foram usadas na construção do projeto:

- [.NET](https://dotnet.microsoft.com/en-us/)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [Postman](https://www.postman.com/downloads/)

## Readme - Próximo Desafio

[Readme Desafio IV](README4.md)

## Licença

Licensed under the [MIT license](LICENSE).

## Autor

<b>Claudinei Santos</b>
============> Discorrer sobre dificuldades e aprendizados nesta sessão.

Encontrei dificuldades em:

- [x] Entender a estrutura Onion Architecture;
- [x] Criar as Class Library através do CLI do .NET;
- [x] Referenciar as camadas em outra camada;

"""Consegui compreender um pouco mais da arquitetura depois de superar as dificuldades acima citadas.
Em meio a minhas horas de estudo pude aprender algumas coisas que eu gostei e acabei implementando no desafio. Como por exemplo:

- [x] Uma Entidade Base;
- [x] Data de Criação do Customer;
- [x] Data de Atualização do Customer;
- [x] Criar um Modulo para cadastrar meus services que fazem injeções de dependencias;"""

Discorrer sobre dificuldades e aprendizados nesta sessão. <============

[![Linkedin Badge](https://img.shields.io/badge/-Claudinei-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/claudinei-santos-ti/)](https://www.linkedin.com/in/claudinei-santos-ti/)
[![Gmail Badge](https://img.shields.io/badge/-santos.devclaudinei@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:santos.devclaudinei@gmail.com)](mailto:claudinei.santos@warren.com.br)