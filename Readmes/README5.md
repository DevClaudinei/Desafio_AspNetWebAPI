## Contexto

Implementar biblioteca de acesso-a-dados de acordo com a versão atual do .Net na camada de domínio/serviço substituindo repository do EFCore pelo UnitOfWork e RepositoryFactory providenciados pela biblioteca.

## Critérios para aceitação

* UnitOfWork para operações de inserir ou atualizar dados;

* RepositoryFactory para consultar dados.;

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

As seguintes ferramentas e/ou pacotes foram usados na construção e utilização no projeto:

- [.NET](https://dotnet.microsoft.com/en-us/)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [EntityFramework](https://docs.microsoft.com/pt-br/ef/)
- [AutoMapper](https://automapper.org/)
- [Docker](https://www.docker.com/)
- [MySQL WorkBench](https://www.mysql.com/products/workbench/)
- [Postman](https://www.postman.com/downloads/)

## Readme - Próximo Desafio

[Readme Desafio VI](README6.md)

## Licença

Licensed under the [MIT license](LICENSE).

## Autor

<b>Claudinei Santos</b>

Encontrei dificuldades em:

- [x] Entender como funciona, como inserir no projeto e como utilizar a biblioteca de acesso a dados;
- [x] Compreender como funciona UnitOfWork e adequar às necessidades do desáfio;
- [x] Pensar como implementar o RepositoryFactory para realizar buscas de informações no banco de dados;
- [x] Resolução de problemas referentes a configurações do contexto;

"""Este desafio me instigou a entender uma gama maior de assuntos mais profundos, que de certa forma considerei  "complexos", mas isso não foi de maneira alguma considerado uma coisa ruim, pois eu precisei buscar artigos e vídeos que me dessem uma base de padrões de projeto como sobre padrão criacional como por exemplo o Factory Method. Constato que o tempo demasiado neste desafio se deu por conta de tentar implementar algo mais complexo do que necessitava na verdade, talvez por ter me confundido com o que me foi pedido acabei delegando a maior parte do tempo a buscar conteúdos para que fossem a base do meu conhecimento na resoluçao deste problema e de problemas futuros. Ao final do desafio estou feliz pelo que entreguei, por ter atendido meus colegas de equipe quando esses tiveram dificuldade em um conceito, na resolução de um problema ou outra situação qualquer e de ter feito o caminho inverso e ter tido um resultado muito satisfatório dos mesmos.

Implementações realizadas nesse desafio:

- [x] Utilização da biblioteca de acesso a dados;
- [x] Inserção e uso do pacote UnitofWork para operações de inserir, atualizar e excluir dados;
- [x] Inserção e uso do pacote RepositoryFactory para operações de consultas de dados;
- [x] Aplicação de novas configurações para o contexto de dados;
"""

[![Linkedin Badge](https://img.shields.io/badge/-Claudinei-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/claudinei-santos-ti/)](https://www.linkedin.com/in/claudinei-santos-ti/)
[![Gmail Badge](https://img.shields.io/badge/-santos.devclaudinei@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:santos.devclaudinei@gmail.com)](mailto:claudinei.santos@warren.com.br)