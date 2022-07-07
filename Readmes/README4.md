## Contexto

Nesse desafio criamos um banco de dados MySQL utilizando docker e substituir estrutura de banco de dados em memória dos desafios anteriores.

## Documentação técnica do desafio

Para consultas/queries, podemos utilizar o MYSQL workbench;

Será necessário criar um novo projeto classlib(Infrastructure.Data) que conterá todas as migrations, mappings e DbContext.

* Subir container MySql com docker-compose;

* Utilizar EFCore(Entity Framework Core);

* Configuração banco de dados com pacote:

``` Pomelo.EntityFrameworkCore.MySql ```

* Para executar Migrations será utilizado o pacote Nuget:

``` Microsoft.EntityFrameworkCore.Tools ```

## Critérios para aceitação

* ConnectionString do banco de dados deverá estar contida na appsettings.json da camada de Api;

* Aplicar Configurations/Mappings via Assembly;

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
- [Postman](https://www.postman.com/downloads/)

## Readme - Próximo Desafio

[Readme Desafio IV](README5.md)

## Licença

Licensed under the [MIT license](LICENSE).

## Autor

<b>Claudinei Santos</b>

Encontrei dificuldades em:

- [x] Configurar para que o AddDbContext carregasse as informações da ConnectionString utilizando a classe DbConfiguration;
- [x] Fazer com que a Aplicação se comunicasse com o MySql;
- [x] Criação do CustomerMapping;
- [x] Criação das migrations;

"""Este desafio me instigou a buscar mais conhecimento téorico que em mídias visuais (vídeos), pois senti a necessidade de compreender o básico para que eu pudesse pensar em como solucionar questẽoes referentes a troca de uma lista por um banco de dados.
Percebi que apesar de todo o tempo que fiquei travado tentando conectar a aplicação ao banco de dados e outros acima citados, foi imprescindível para que eu pudesse consolidar meu conhecimento e transmitir um pouco do que eu sabia ou tinha vivenciado para os meus colegas de stack. Me sinto feliz por ter ajudado as pessoas na hora que elas precisaram e de ter sido ajudado pelos mesmos quando eu não conseguia enxergar o obvio.

Implementações realizadas nesse desafio:

- [x] Criação de mappings;
- [x] Criação das migrations;
- [x] Delegar ao banco a atribuição dos IDs do tipo Guid;
- [x] Criação da classe DbConfiguration para evitar o uso do arquivo program.cs;
"""

[![Linkedin Badge](https://img.shields.io/badge/-Claudinei-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/claudinei-santos-ti/)](https://www.linkedin.com/in/claudinei-santos-ti/)
[![Gmail Badge](https://img.shields.io/badge/-santos.devclaudinei@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:santos.devclaudinei@gmail.com)](mailto:claudinei.santos@warren.com.br)