<h1 align="center">Back-End Criação do usuário</h1>

## Descrição do Projeto

<p align="center">Desenvolver o backend utilizando como base as informações enviadas do frontend, informações essas providas do envio de um formulário.
</p>

## Documentação técnica do desafio

### Requisições HTTP:
Para estarmos iniciando nossa aplicação utilizando .NET e estarmos fazendo as requisições para esse servidor, iremos utilizar o postman para estar fazendo as requisições de Post, Get, Patch e Delete.
  
### Rotas:
(/api/customer) => Essa rota receberá uma request no formato de objeto com os dados necessários da etapa 1 e etapa 2: 

- [x] id (Guid)
- [x] full_name (string)
- [x] email (string)
- [x] email_confirmation (string)
- [x] cpf (string)
- [x] cellphone (string)
- [x] birthdate (Date)
- [x] email_sms (boolean)
- [x] whatsapp (boolean)
- [x] country (string)
- [x] city (string)
- [x] postal_code (string)
- [x] address (string)
- [x] number (number)

### Pontos de atenção
Esses dados deverão ser salvo em um arquivo mock em um diretório exclusivo para os mocks. Deverá ser salvo em uma variável em memória.

Validar corretamente os campos, não permitir nulos, strings em lugar onde o tipo é number e demais validações do formulário de acordo com a tipagem dos campos.

Validação da confirmação de e-mail.

### Retorno da request
A request deverá ter um retorno, retorno esse que deverá ser retornado através do response, seja esse retorno com sucesso ou com erro, para em um futuro esse retorno ser utilizado no frontend

## Pré-requisitos

Antes de começar, você vai precisar ter instalado em sua máquina as seguintes ferramentas:
[Git](https://git-scm.com), [.NET](https://dotnet.microsoft.com/en-us/download). 
Além disto é bom ter um editor para trabalhar com o código como [Visual Studio Code](https://code.visualstudio.com/download)

## 🎲 Rodando o Back End (servidor)

```bash
# Clone este repositório
$ https://github.com/santosclaudinei-warren/AspNetCoreWebAPI

# Acesse a pasta do projeto no terminal/cmd
$ cd DesafioWarren.WebAPI

# Execute a aplicação
$ Executar o comando dotnet run 

# O servidor inciará na porta:4231 
- Utilizando a URL <http://localhost:5208>
```

## 🛠 Tecnologias

As seguintes ferramentas foram usadas na construção do projeto:

- [.NET](https://dotnet.microsoft.com/en-us/)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [Postman](https://www.postman.com/downloads/)

## Readme - Próximo Desafio

[Readme Desafio II](src/Readmes/README2.md)

## Licença

Licensed under the [MIT license](LICENSE).

## Autor

<b>Claudinei Santos</b>

Sinto que neste desafio consegui fazer a entrega dentro do tempo que estipulei, que foi dividido entre estudo da ddocumentação, tutoriais, curso de criação de API com Asp .NET Core e testes. 
Depois do code review, pude atentar para alguns detalhes que passaram batido e na refatoração do mesmo tive a oportunidade de melhorar até outros pontos que não foram mencionados na revisão feita pelo sherpa.

[![Linkedin Badge](https://img.shields.io/badge/-Claudinei-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/claudinei-santos-ti/)](https://www.linkedin.com/in/claudinei-santos-ti/)
[![Gmail Badge](https://img.shields.io/badge/-santos.devclaudinei@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:santos.devclaudinei@gmail.com)](mailto:claudinei.santos@warren.com.br)