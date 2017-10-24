using System;
using System.IO;

namespace sistema_vendas
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int opcao = 0;
            
                do{
                //Mostra um menu de opções para o usuário
                Console.WriteLine("Digite a opção");
                Console.WriteLine("1 - Cadastrar Cliente");
                Console.WriteLine("2 - Cadastrar Produto");
                Console.WriteLine("3 - Realizar Venda");
                Console.WriteLine("4 - Extrato Cliente");
                Console.WriteLine("9 - Sair");

                //Recebe a opção do usuário
                opcao =  Int16.Parse(Console.ReadLine());
                
                //Verifica qual opção o usuário informou
                switch(opcao){
                    case 1:
                        CadastrarCliente();
                        break;
                    case 2:
                        CadastrarProduto();
                        break;
                    case 3:
                        RealizarVenda();
                        break;
                    case 4:
                        ExtratoCliente();
                        break;
                    case 9:{
                        //Pergunta para o usuário se ele realmente deseja sair
                        Console.WriteLine("Deseja realmente sair(s ou n)");
                        //Obtem a opção do usuário
                        string sair = Console.ReadLine();
                        //Verifica se ele digitou s
                        if(sair.ToLower().Contains("s"))
                            Environment.Exit(0);
                        else if(!sair.ToLower().Contains("n"))
                        {
                            opcao = 0;
                            Console.WriteLine("Opção Inválida");
                        }
                        else{
                            opcao = 0;
                        }
                        break;
                    }
                    default:
                        Console.WriteLine("Opção Inválida");
                        break;
                }
                //fica no laço até o usuário digitar 9
                }while(opcao != 9);
            }
            catch (System.Exception e)
            {
                //Caso ocorra algum erro grava no arquivo de erros
                GravarErro("Main", e.Message);
            }
        }

        /// <summary>
        /// Validação de cpf
        /// </summary>
        /// <param name="cpf">Parametro do tipo string recebe o cpf</param>
        /// <returns>Retorna true ou false</returns>
        static bool ValidarCPF(string cpf){
            //Retira os pontos e traços
            cpf = cpf.Trim().Replace(".", "").Replace("-","");

            //Verifica se tem 11 digitos o parametro passado, caso não tenha retorna falso
            if (cpf.Length != 11){
                return false;
            }

            //Verifica se o cpf digitado não possui a sequência de números informada
            if(cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222"
             || cpf == "33333333333" || cpf == "44444444444" || cpf == "55555555555"
             || cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" || cpf == "99999999999"){
                 return false;
             }

            //cria um array de int para validar o primeiro digito
            int[] multiplicador1 = new int[9]{10,9,8,7,6,5,4,3,2};
            //cria um array de int para validar o segundo digito
            int[] multiplicador2 = new int[10]{11,10,9,8,7,6,5,4,3,2};


             string tempCpf, digito;
             int soma =0,resto =0;

            //armazena na váriavel tempCpf os 9 primeiros digitos do cpf passado como parametro
             tempCpf = cpf.Substring(0,9);

            //percorre o array multiplicando os digitos do cpf com a posição do array e soma
            for (int i = 0; i < 9; i++)
            {
                soma += Convert.ToInt16(tempCpf[i].ToString())  * multiplicador1[i];
            }

            //armazena o resto da divisão da soma por 11
            resto = soma % 11;

            //Caso o resto seja menor que 2 atribui 0, caso contrário atribui 11 - resto para obter primeiro número
            if(resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            //atribui a digito o resto
            digito = resto.ToString();
            //concatena o tempCpf com o digito
            tempCpf = tempCpf + digito;

            soma = 0;
            //Percorre o tempcpf contatenado e multiplica pelos valores do array
            for (int i = 0; i < 10; i++)
            {
                soma += Convert.ToInt16(tempCpf[i].ToString())  * multiplicador2[i];
            }

            //armazena o resto da divisão da soma por 11
            resto = soma % 11;

            //concatena o tempCpf com o digito
            if(resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito  =digito + resto.ToString();

            //Verifica se os ultimos 2 digitos obtidos são iguais aos do cpf passado
            return cpf.EndsWith(digito);
        }

        /// <summary>
        /// Validação de CNPJ
        /// </summary>
        /// <param name="cnpj">Recebe como parametro de entrada um string com o valor do cnpj a ser validado</param>
        /// <returns>Retorna true ou false</returns>
        static bool ValidarCNPJ(string cnpj){

            //Retira os caracteres especiais do CNPJ
            cnpj = cnpj.Trim().Replace(".", "").Replace("-","");

            //Verifica se o CNPJ possui 14 caracteres
            if (cnpj.Length != 14){
                return false;
            }

            //Declara um array com valores a serem multiplicados para encontrar o primeiro caractere
            int[] multiplicador1 = new int[12]{5,4,3,2,9,8,7,6,5,4,3,2};
            //Declara um array com valores a serem multiplicados para encontrar o segundo caractere
            int[] multiplicador2 = new int[13]{6,5,4,3,2,9,8,7,6,5,4,3,2};


            string tempCnpj, digito;
            int soma =0,resto =0;

            //Atribui a variavel os 12 primeiros caracteres do cnpj
            tempCnpj = cnpj.Substring(0,12);

            //Percorre os 12 caracteres e otem a soma
            for (int i = 0; i < 12; i++)
             {
                 //multiplica o valor do array na poição i ao caracter na posição i
                 soma += Convert.ToInt16(tempCnpj[i].ToString())  * multiplicador1[i];
             }

            //obtêm o resto da divisão da soma por 11
            resto = soma % 11;

            //Caso o resto seja menor que 2 atribui 0 e caso seja maior atribui ao valor 11 - resto
            if(resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            //Atribui o valor do resto a variável digito
            digito = resto.ToString();
            //Concatena o cnpj com 12 caracteres ao resto para validar o segundo dígito
            tempCnpj = tempCnpj + digito;

            soma = 0;
            //Percorre os 13 caracteres e otem a soma
            for (int i = 0; i < 13; i++)
            {
                 //multiplica o valor do array na poição i ao caracter na posição i
                 soma += Convert.ToInt16(tempCnpj[i].ToString())  * multiplicador2[i];
            }

            //Caso o resto seja menor que 2 atribui 0 e caso seja maior atribui ao valor 11 - resto
            resto = soma % 11;

            //Caso o resto seja menor que 2 atribui 0 e caso seja maior atribui ao valor 11 - resto
            if(resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            //Atribui o valor do resto a variável digito
            digito = resto.ToString();

            //Verifica se o digito é igual aos do cnpj, caso seja retorna true caso contrário retorna false
            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// Verifica se existe um produto cadastrado pelo código
        /// </summary>
        /// <param name="codigoProduto">Parametro de entrada do tipo de string com o código do produto a ser verificado</param>
        /// <returns>Retorna true ou false</returns>
        static bool VerificaProdutoCadastrado(string codigoProduto){

            try
            {
                //Verifica se arquivo existe, caso não exista produto não cadastrado
                if (!File.Exists("produtos.txt"))
                {
                    return false;
                }

                //Caso arquivo exista obtem todos os produtos do arquivo
                string[] produtos = File.ReadAllLines("produtos.txt");
                string[] arrayproduto;
                //percorre o array de produtos
                foreach (var produto in produtos)
                {
                    //Split cria um array e atribui o mesmo a váriavel arrayproduto
                    arrayproduto = produto.Split(";");
                    //verifica se o produto já foi cadastrado
                    if (arrayproduto[0] == codigoProduto)
                    {
                        //Caso exista retorna true
                        return true;
                    }
                }

                //Caso não exista retorna false
                return false;
            }
            catch (System.Exception e)
            {
                //Caso ocorra algum erro salva em um arquivo de erros
                GravarErro("VerificaProdutoCadastrado",e.Message );
                throw;
            }
        }

        /// <summary>
        /// Verifica se existe um cliente cadastrado pelo documento(cpf/cnpj)
        /// </summary>
        /// <param name="documento">Parametro do tipo string com o valor do documento(cpf/cnpj)</param>
        /// <returns>Retorna true caso exista e false se não existe</returns>
        static bool VerificaClienteCadastrado(string documento){

            try
            {
                //Verifica se arquivo existe, caso não exista produto não cadastrado
                if (!File.Exists("clientes.txt"))
                {
                    return false;
                }
                //Caso arquivo exista obtem todos os produtos do arquivo
                string[] clientes = File.ReadAllLines("clientes.txt");
                string[] arraycliente;
                //percorre o array de produtos
                foreach (var cliente in clientes)
                {
                     //Split cria um array e atribui o mesmo a váriavel arrayproduto
                    arraycliente = cliente.Split(";");
                    //verifica se o produto já foi cadastrado
                    if (arraycliente[0] == documento)
                    {
                        //Caso exista retorna true
                        return true;
                    }
                }
                
                //Caso não exista retorna false
                return false;
            }
            catch (System.Exception e)
            {
                //Caso ocorra algum erro salva em um arquivo de erros
                GravarErro("VerificaClienteCadastrado",e.Message );
                throw;
            }
        }


        /// <summary>
        /// Efetua o cadastro do cliente em um arquivo texto
        /// </summary>
        static void CadastrarCliente(){
            try
            {
                //Obtêm o nome do cliente
                Console.WriteLine("Digite o nome do cliente");
                string nome = Console.ReadLine();

                //Obtêm o email do cliente
                Console.WriteLine("Digite o email do cliente");
                string email = Console.ReadLine();


                string opcaopfpj = "";

                //Entra no laço de repetição perguntando se é uma pessoa física ou jurídica
                do{                  

                    //Obtem o tipo de pessoa
                    Console.WriteLine("Digite 1 para pessoa física  e 2 para pessoa jurídica");
                    opcaopfpj = Console.ReadLine();

                    //Caso opção seja diferente de 1 ou 2 mostra opção inválida
                    if(opcaopfpj != "1" && opcaopfpj != "2"){
                        Console.WriteLine("Opção invalida");
                    }

                    //Fica no laço de repetição até digitar a opção certa
                }while(opcaopfpj != "1" && opcaopfpj != "2");

                string documento;
                bool documentovalido = false;

                //Entra no laço de repetição e fica no mesmo até digitar um documento válido
                do{

                    if(opcaopfpj == "1"){
                        //Caso opção digitada seja 1 pede o cpf
                        Console.WriteLine("Digite seu CPF");
                        documento = Console.ReadLine();
                        documentovalido = ValidarCPF(documento);

                        if(!documentovalido)
                            Console.WriteLine("CPF Inválido");
                    }
                    else{
                        //Caso opção digitada seja 2 pede o CNPJ
                        Console.WriteLine("Digite seu CNPJ");
                        documento = Console.ReadLine();

                        documentovalido = ValidarCNPJ(documento);

                        if(!documentovalido)
                            Console.WriteLine("CNPJ Inválido");
    	            }
                    //fica no laço até digitar um número de documento válido(cpf/cnpj)
                }while(documentovalido == false);

                //Grava as informações digitadas pelo usuário em um arquivo texto
                StreamWriter sr = new StreamWriter("clientes.txt", true);
                sr.WriteLine(documento + ";" + nome + ";" + email );
                sr.Close();

                //Informa ao usuário que o cliente foi cadastrado
                Console.WriteLine(" Cliente " + nome + " cadastrado");
            }
            catch (Exception e)
            {
                GravarErro("CadastrarCliente", e.Message);
            } 
        }  

        /// <summary>
        /// Efetua o cadastro de um produto em arquivo texto
        /// </summary>
        static void CadastrarProduto(){
            try
            {
                string codigoproduto;
                bool produtovalido;

                //Entra no laço de repetição e fica até o usuário digitar um código válido
                do{                   
                    //Obtem o código do produto digitado pelo cliente
                    Console.WriteLine("Digite o código do Produto");
                    codigoproduto = Console.ReadLine();
                    
                    //Verifica se o produto é valido
                    produtovalido = VerificaProdutoCadastrado(codigoproduto);

                    //Caso produto já esteja cadastrado informa ao cliente
                    if(produtovalido)
                        Console.WriteLine("Código produto já cadastrado!");

                    //fica no laço até informar um produto que não tenha sido cadastrado
                }while(produtovalido);

                //Obtem o nome do produto
                Console.WriteLine("Digite o nome do produto");
                string nome = Console.ReadLine();

                //Obtem a descrição do produto
                Console.WriteLine("Digite a descrição do produto");
                string descricao = Console.ReadLine();

                //Obtem o preço do produto
                Console.WriteLine("Digite o preço do produto");
                decimal preco = Convert.ToDecimal(Console.ReadLine());

                //Grava o produto em um arquivo texto
                StreamWriter sr = new StreamWriter("produtos.txt", true);
                sr.WriteLine(codigoproduto + ";" + nome + ";" + descricao + ";" + preco);
                sr.Close();

                //Informa ao usuário que o produto foi cadastrado
                Console.WriteLine(" Produto " + nome + " cadastrado");
            }
            catch (Exception e)
            {
                //Caso ocorra algum erro grava no arquivo de erro
                GravarErro("CadastrarProduto", e.Message);
            } 
        }

        /// <summary>
        /// Realiza a venda do produto ao cliente
        /// </summary>
        static void RealizarVenda(){
            try
            {
                string opcaopfpj = "";

                //fica no laço até digitar uma opção válida
                do{
                    //Obtem se deseja consulta uma pessoa física ou jurídica
                    Console.WriteLine("Digite 1 para pessoa física  e 2 para pessoa jurídica");
                    opcaopfpj = Console.ReadLine();

                    //Caso a opção informada seja diferente de 1 ou 2 informa ao cliente
                    if(opcaopfpj != "1" && opcaopfpj != "2"){
                        Console.WriteLine("Opção invalida");
                    }

                //fica no laço até a opção ser iguala 1 ou igual a 2
                }while(opcaopfpj != "1" && opcaopfpj != "2");

                string documento;
                bool documentovalido = false;

                do{
                    
                    if(opcaopfpj == "1"){
                        //Caso a opção digitada seja 1 pede o cpf
                        Console.WriteLine("Digite seu CPF");
                        documento = Console.ReadLine();

                        //valida o cpf informado
                        documentovalido = ValidarCPF(documento);

                        if(!documentovalido)
                            Console.WriteLine("CPF Inválido");
                    }
                    else{
                        //caso a opção digitada seja 2 pede o cnpj
                        Console.WriteLine("Digite seu CNPJ");
                        documento = Console.ReadLine();

                        //valida o cnpj
                        documentovalido = ValidarCNPJ(documento);

                        if(!documentovalido)
                            Console.WriteLine("CNPJ Inválido");
    	            }

                    //Fica no laço até digitar um número de documento válido
                }while(!documentovalido);

                //Verifica se o cliente informado foi cadastrado
                bool clientecadastrado = VerificaClienteCadastrado(documento);

                //Caso cliente informado não tenha sido cadastrado pede para cadastrar um novo cliente
                if(!clientecadastrado)
                {
                    Console.WriteLine("Cliente não cadastrado, cadastre um novo cliente");
                    CadastrarCliente();
                }

                #region Busca dados Cliente
                    //Le no arquivo cliente.txt todas as linhas e atribui o valor a variavel clientes
                    string[] clientes = File.ReadAllLines("clientes.txt");
                    string[] cliente = null;
                    //Percorre o array clientes
                    foreach (var item in clientes)
                    {
                        //atribui a variavel cliente um array com a linha percorrida no foreach
                        cliente = item.Split(";");
                        //verifica se na posição 0 o valor é igual ao documento informado
                        if(cliente[0] == documento)
                        {
                            //Caso seja encontrado mostra para o usuário os dados do cliente
                            Console.WriteLine("Documento: " + cliente[0]);
                            Console.WriteLine("Nome: " + cliente[1]);
                            Console.WriteLine("Email: " + cliente[2]);
                            break;
                        }
                    }
                #endregion

                #region Lista Produtos
                    //Le no arquivo produtos.txt todas as linhas e atribui o valor a variavel produtos
                    string[] produtos = File.ReadAllLines("produtos.txt");
                    string[] produto = null;
                    //Percorre o array produtos
                    foreach (var item in produtos)
                    {
                        //atribui a variavel produto um array com a linha percorrida no foreach
                        produto = item.Split(";");

                        //Mostra ao cliente os produtos cadastrado com os espaços definidos em PadRight
                        Console.WriteLine(produto[0].PadRight(15) + produto[1].PadRight(25) + produto[2].PadRight(35) + produto[3].PadRight(20));
                    }
                #endregion

                string codigoproduto;
                bool produtoencontrado = false;

                //fica no laço de repetição até digitar um código de produto válido
                do{
                    //Obtem qual produto o cliente deseja comprar
                    Console.WriteLine("Digite o código do produto");
                    codigoproduto  = Console.ReadLine();

                    //verifica se o código do produto digitado é um produto cadastrado
                    produtoencontrado = VerificaProdutoCadastrado(codigoproduto);

                    //Caso o código do produto não tenha sido encontrado informa ao cliente e pede um novo código
                    if(!produtoencontrado)
                        Console.WriteLine("Código não encontrado, informe um código válido");

                    //Fica no laço até ser um produto válido
                }while(!produtoencontrado);

                #region Encontra produto
                //Caso produto seja válido percorre o array de produtos e mostra na tela para  usuário
                   foreach (var item in produtos)
                    {
                        //atribui a variável produto um array da linha percorrida
                        produto = item.Split(";");
                        //Caso encontre o produto informa ao cliente
                        if(produto[0] == codigoproduto){
                            Console.WriteLine("Produto escolhido " + produto[0].PadRight(15) + produto[1].PadRight(25) + produto[2].PadRight(35) + produto[3].PadRight(20));
                            break;
                        }
                        
                    }
                #endregion

                //Salva em um arquivo texto a venda
                StreamWriter sw = new StreamWriter("vendas.txt", true);
                sw.WriteLine(cliente[0] + ";" + cliente[1] + ";" + produto[0]+ ";" + produto[1]+ ";" + produto[2]+ ";" + produto[3] );
                sw.Close();
            }
            catch (System.Exception e)
            {
                GravarErro("RealizarVenda", e.Message);
            }
        }

        /// <summary>
        /// Obtem as vendas realizadas para um determinado cliente
        /// </summary>
        static void ExtratoCliente(){
            String opcaopfpj = "";


                //fica no laço até digitar uma opção válida
                do{
                    //Obtem se deseja consulta uma pessoa física ou jurídica
                    Console.WriteLine("Digite 1 para pessoa física  e 2 para pessoa jurídica");
                    opcaopfpj = Console.ReadLine();

                    //Caso a opção informada seja diferente de 1 ou 2 informa ao cliente
                    if(opcaopfpj != "1" && opcaopfpj != "2"){
                        Console.WriteLine("Opção invalida");
                    }

                //fica no laço até a opção ser iguala 1 ou igual a 2
                }while(opcaopfpj != "1" && opcaopfpj != "2");

                string documento;
                bool documentovalido = false;

                do{
                    
                    if(opcaopfpj == "1"){
                        //Caso a opção digitada seja 1 pede o cpf
                        Console.WriteLine("Digite seu CPF");
                        documento = Console.ReadLine();

                        //valida o cpf informado
                        documentovalido = ValidarCPF(documento);

                        if(!documentovalido)
                            Console.WriteLine("CPF Inválido");
                    }
                    else{
                        //caso a opção digitada seja 2 pede o cnpj
                        Console.WriteLine("Digite seu CNPJ");
                        documento = Console.ReadLine();

                        //valida o cnpj
                        documentovalido = ValidarCNPJ(documento);

                        if(!documentovalido)
                            Console.WriteLine("CNPJ Inválido");
    	            }

                    //Fica no laço até digitar um número de documento válido
                }while(!documentovalido);

                //Verifica se o arquivo vendas existe, caso não exista não foi feita vendas ainda
                if (!File.Exists("vendas.txt"))
                {
                    Console.WriteLine("Não foram efetuadas vendas!!!");
                }
                else
                {
                    //Le no arquivo produtos.txt todas as linhas e atribui o valor a variavel produtos
                    string[] vendas = File.ReadAllLines("vendas.txt");
                    string[] arrayvenda;
                    //percorre o array de vendas
                    foreach (var produto in vendas)
                    {
                        //atribui a variável produto um array da linha percorrida
                        arrayvenda = produto.Split(";");
                        //Verifica linha por linha, caso encontre o documento na linha na posição 0 do array informa ao cliente
                        if (arrayvenda[0] == documento)
                        {
                            Console.WriteLine(arrayvenda[0].PadRight(15) + arrayvenda[1].PadRight(15) + arrayvenda[2].PadRight(25) + arrayvenda[4].PadRight(25));
                        }
                    }
                }
        }
        
        /// <summary>
        /// Grava os erros do projeto em um arquivo texto
        /// </summary>
        /// <param name="funcao">Qual a função que ocorreu o erro</param>
        /// <param name="erro">Qual a mensagem de erro</param>
        static void GravarErro(string funcao, string erro){
                try
                {
                    //Informa ao usuário que ocorreu um erro
                    Console.WriteLine("Ocorreu um erro - Contacte o Administrador");
                    //Abre um arquivo textp
                    StreamWriter sr = new StreamWriter("logerro.txt", true);
                    //Grava as informações no arquivo erro
                    sr.WriteLine(DateTime.Now + " - " + funcao + " - " + erro );
                    //fecha o arquivo, caso não feche o arquivo não será salvo
                    sr.Close();
                }
                catch (System.Exception)
                {
                    
                    Console.WriteLine("Ocorreu um erro - Contacte o Administrador");
                }
        }
    }
}
