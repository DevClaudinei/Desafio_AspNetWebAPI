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
$ https://github.com/santosclaudinei-warren/Desafio_AspNetWebAPI

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
- [AutoMapper](https://automapper.org/)
- [Postman](https://www.postman.com/downloads/)

## Readme - Próximo Desafio

[Readme Desafio IV](README4.md)

## Licença

Licensed under the [MIT license](LICENSE).

## Autor

<b>Claudinei Santos</b>

Encontrei dificuldades em:

- [x] Entender como funcionava a criação de um mapeamento de uma entidade para outra;
- [x] Compreender como fazer uso dos perfis de mapeamento criados e seus retornos na camada de AppService;
- [x] Criar entidades a serem expostas e utilizar as mesmas;
- [x] Remover uso de Regex para validação de CPF e implementar novo método que verificasse se um CPF era válido;
- [x] Elaborar verificação do campo de tipo string para checar se fullName tem pelo menos um nome válido;

"""Ao final do desafio fiquei muito feliz, pois acabei adquirindo uma grande quantidade de conhecimento, pois ao inserir o conceito de mapeamento utilizando o pacote AutoMapper gerou vários problemas no código que precisavam ser resolvidos para a API voltar a funcionar.
No início pensei que não conseguiria resolver ou pensar em como resolver, mas aos poucos eu fui dizendo pra mim mesmo que eu era capaz, então iniciei pelos erros aparentemente mais fáceis e foi fluindo.
Enfrentar esses problemas me fizeram ter mais segurança no que eu estava fazendo, me deram novos aprendizados e passar por eles me permitiram saber como socorrer alguns dos colegas que depois viriam passar pelo mesmo.
Implementações realizadas nesse desafio:

- [x] Retorno em tupla (para retorno de um booleano e uma mensagem de acordo com booleano) para criação e atualização de um Customer;
- [x] Criação de entidades para serem utilizadas numa requisição (CreateCustomerRequest e UpdateCustomerRequest) e para resposta de uma requisição (CustomerResult);
- [x] Validação dos campos para as entidades utilizadas ao tentar criar ou atualizar um Customer;
- [x] Elaboração de uma verificação mais assertiva para verificar se um Customer é maior de idade;
- [x] Verificação para checar se o email e/ou CPF já esta cadastrado no banco para outro usuário;
- [x] Inserção de classes contendo configurações para serem referenciadas no Program.cs (Arquivo de configurações da aplicação);
- [x] Com a constatação de que para atualizar um Customer precisava checar se os campos email e o CPF já existiam no banco e que aconteceria uma duplicação de código. Tive a percepção que deveria fazer a reutilização de código criando um metódo comum para esta verificação que seria então utilizada tanto para criar quanto para atualizar um Customer;
- [x] Criar validação para campos do tipo string com o intuito de verificar por exemplo se uma pessoa tem nome e no mínimo um sobrenome;
"""

[![Linkedin Badge](https://img.shields.io/badge/-Claudinei-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/claudinei-santos-ti/)](https://www.linkedin.com/in/claudinei-santos-ti/)
[![Gmail Badge](https://img.shields.io/badge/-santos.devclaudinei@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:santos.devclaudinei@gmail.com)](mailto:claudinei.santos@warren.com.br)