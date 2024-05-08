# ManPowerAPI
.NETAPI

Configuração de ambiente:

API{

 -Certifique-se de ter o .NET SDK instalado na sua máquina.
 
 -Clone o repositório com: git clone <url_do_repositório>
 
 -Abra o arquivo appsettings.json e atualize a conexão com o banco de dados, se necessário.
 
 -Abra o Console do Gerenciador de Pacotes do Visual Studio (PowerShell) e execute:
 
   1.add-migration v1
   
   2.update-database
}


Documentação da API ManPower

Visão Geral

Esta API fornece endpoints para autenticação de usuário, criação de usuário, gerenciamento de registros de controle de tempo e listagem de usuários.

URL Base: /api/user

Autenticação

Autenticar Usuário

Autentica um usuário com suas credenciais e recupera um token JWT para acessar rotas protegidas.

Endpoint: POST/

authenticateRequisição:

Método: POST

Corpo:

userName (string, obrigatório): O nome de usuário do usuário.

password (string, obrigatório): A senha do usuário.

Respostas:


200 OK
{ "Token": "Token JWT", "Message": "Login bem-sucedido!" }


400 Bad Request
{ "Message": "Corpo da solicitação inválido" }


404 Not Found
{ "Message": "Usuário não encontrado" }


500 Internal Server Error
{ "Message": "Erro interno do servidor: <mensagem_de_erro>" }


Gerenciamento de Usuários

Criar Usuário

Cria uma nova conta de usuário.

Endpoint: POST/

createRequisição:
Método: POST

Corpo:

userName (string, obrigatório): O nome de usuário do usuário.

password (string, obrigatório): A senha do usuário.

Respostas:


200 OK
{ "Message": "Usuário criado com sucesso" }


400 Bad Request
{ "Message": "Dados de usuário inválidos" }

ou

{ "Message": "A senha não atende aos requisitos de segurança" }


500 Internal Server Error
{ "Message": "Falha ao criar usuário: <mensagem_de_erro>" }


Obter Todos os Usuários
Recupera uma lista de todos os usuários.



Endpoint: GET/

Requisição:

Método: GET

Respostas:


200 OK
Corpo: Array de objetos de usuário.


401 Unauthorized
{ "Message": "Não autorizado" }


500 Internal Server Error
{ "Message": "Erro interno do servidor: <mensagem_de_erro>" }


Gerenciamento de Registros de Controle de Tempo

Obter Registro de Controle de Tempo por Usuário

Recupera registros de controle de tempo para um usuário específico dentro de um intervalo de datas.


Endpoint: GET /getkbyuser 

Requisição:

Método: GET

Parâmetros de Consulta:

userId (int, obrigatório): O ID do usuário.

initialDate (DateTime, obrigatório): A data inicial do intervalo.

finalDate (DateTime, obrigatório): A data final do intervalo.

Respostas:

200 OK
Corpo: Registros de controle de tempo para o usuário especificado.


500 Internal Server Error
{ "Message": "Houve uma falha ao encontrar seu registro de entrada/saída: <mensagem_de_erro>" }


Criar Registro de Controle de Tempo

Cria um novo registro de controle de tempo para um usuário.


Endpoint: POST /createtkRequisição:

Método: POST

Corpo: userId (int, obrigatório): O ID do usuário.

Respostas:


200 OK
{ "Message": "Horas definidas com sucesso" }


400 Bad Request
{ "Message": "Você já registrou o número máximo de horas" }


500 Internal Server Error
{ "Message": "Houve uma falha ao registrar sua entrada/saída: <mensagem_
