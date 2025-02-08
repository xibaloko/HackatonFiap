# 🚀 Hackaton

A aplicação que pode ser facilmente executada utilizando **Docker Compose**.

## 📌 Pré-requisitos

Antes de iniciar a aplicação, certifique-se de ter os seguintes itens instalados em sua máquina:

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

## 🔧 Como subir a aplicação

1. Clone este repositório:

   ```sh
   git clone https://github.com/seu-usuario/seu-repositorio.git
   cd seu-repositorio
   ```

2. Execute o seguinte comando para iniciar os containers:
 
   O arquivo `docker-compose.yml` localizado na raiz do projeto.
   ```sh
   docker-compose up -d
   ```

   O parâmetro `-d` executa os containers em modo **detach** (em segundo plano).

3. Para verificar se os containers estão rodando:

   ```sh
   docker ps
   ```

4. Para encerrar os containers:

   ```sh
   docker-compose down
   ```

## 🛠 Personalização

Caso precise modificar algo, edite o arquivo `docker-compose.yml` localizado na raiz do projeto.

