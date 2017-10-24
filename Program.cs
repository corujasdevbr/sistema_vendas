﻿using System;
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
                Console.WriteLine("Digite a opção");
                Console.WriteLine("1 - Cadastrar Cliente");
                Console.WriteLine("2 - Cadastrar Produto");
                Console.WriteLine("3 - Realizar Venda");
                Console.WriteLine("4 - Extrato Cliente");
                Console.WriteLine("9 - Sair");

                opcao =  Int16.Parse(Console.ReadLine());
                
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
                        Console.WriteLine("Deseja realmente sair(s ou n)");
                        string sair = Console.ReadLine();
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

                }while(opcao != 9);
            }
            catch (System.Exception e)
            {
                GravarErro("Main", e.Message);
            }
        }

        static bool ValidarCPF(string cpf){
            cpf = cpf.Trim().Replace(".", "").Replace("-","");

            if (cpf.Length != 11){
                return false;
            }

            if(cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222"
             || cpf == "33333333333" || cpf == "44444444444" || cpf == "55555555555"
             || cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" || cpf == "99999999999"){
                 return false;
             }

             int[] multiplicador1 = new int[9]{10,9,8,7,6,5,4,3,2};
             int[] multiplicador2 = new int[10]{11,10,9,8,7,6,5,4,3,2};

             string tempCpf, digito;
             int soma =0,resto =0;

             tempCpf = cpf.Substring(0,9);

             for (int i = 0; i < 9; i++)
             {
                 soma += Convert.ToInt16(tempCpf[i].ToString())  * multiplicador1[i];
             }

             resto = soma % 11;

            if(resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;

            soma = 0;
             for (int i = 0; i < 10; i++)
             {
                 soma += Convert.ToInt16(tempCpf[i].ToString())  * multiplicador2[i];
             }

             resto = soma % 11;

            if(resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito  =digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        static bool ValidarCNPJ(string cnpj){
            cnpj = cnpj.Trim().Replace(".", "").Replace("-","");

            if (cnpj.Length != 14){
                return false;
            }

           

             int[] multiplicador1 = new int[12]{5,4,3,2,9,8,7,6,5,4,3,2};
             int[] multiplicador2 = new int[13]{6,5,4,3,2,9,8,7,6,5,4,3,2};

             string tempCnpj, digito;
             int soma =0,resto =0;

             tempCnpj = cnpj.Substring(0,12);

             for (int i = 0; i < 12; i++)
             {
                 soma += Convert.ToInt16(tempCnpj[i].ToString())  * multiplicador1[i];
             }

             resto = soma % 11;

            if(resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;

            soma = 0;
             for (int i = 0; i < 13; i++)
             {
                 soma += Convert.ToInt16(tempCnpj[i].ToString())  * multiplicador2[i];
             }

             resto = soma % 11;

            if(resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            return cnpj.EndsWith(digito);
        }

        static bool VerificaProdutoCadastrado(string codigoProduto){

            try
            {
                if (!File.Exists("produtos.txt"))
                {
                    return false;
                }

                string[] produtos = File.ReadAllLines("produtos.txt");
                string[] arrayproduto;
                foreach (var produto in produtos)
                {
                    arrayproduto = produto.Split(";");
                    if (arrayproduto[0] == codigoProduto)
                    {
                        return true;
                        break;
                    }
                }

                return false;
            }
            catch (System.Exception e)
            {
                GravarErro("VerificaProdutoCadastrado",e.Message );
                throw;
            }
        }

        static bool VerificaClienteCadastrado(string documento){

            try
            {
                if (!File.Exists("clientes.txt"))
                {
                    return false;
                }

                string[] produtos = File.ReadAllLines("clientes.txt");
                string[] arrayproduto;
                foreach (var produto in produtos)
                {
                    arrayproduto = produto.Split(";");
                    if (arrayproduto[0] == documento)
                    {
                        return true;
                        break;
                    }
                }

                return false;
            }
            catch (System.Exception e)
            {
                GravarErro("VerificaClienteCadastrado",e.Message );
                throw;
            }
        }


        static void CadastrarCliente(){
            try
            {
                Console.WriteLine("Digite o nome do cliente");
                string nome = Console.ReadLine();

                Console.WriteLine("Digite o email do cliente");
                string email = Console.ReadLine();

                string opcaopfpj = "";

                do{
                    
                    Console.WriteLine("Digite 1 para pessoa física  e 2 para pessoa jurídica");
                    opcaopfpj = Console.ReadLine();
                    if(opcaopfpj != "1" && opcaopfpj != "2"){
                        Console.WriteLine("Opção invalida");
                    }

                }while(opcaopfpj != "1" && opcaopfpj != "2");

                string documento;
                bool documentovalido = false;

                do{
                    if(opcaopfpj == "1"){
                        Console.WriteLine("Digite seu CPF");
                        documento = Console.ReadLine();
                        documentovalido = ValidarCPF(documento);

                        if(!documentovalido)
                            Console.WriteLine("CPF Inválido");
                    }
                    else{
                        Console.WriteLine("Digite seu CNPJ");
                        documento = Console.ReadLine();

                        documentovalido = ValidarCNPJ(documento);

                        if(!documentovalido)
                            Console.WriteLine("CNPJ Inválido");
    	            }
                }while(!documentovalido);


                StreamWriter sr = new StreamWriter("clientes.txt", true);
                sr.WriteLine(documento + ";" + nome + ";" + email );
                sr.Close();

                Console.WriteLine(" Cliente " + nome + " cadastrado");
            }
            catch (Exception e)
            {
                GravarErro("CadastrarCliente", e.Message);
            } 
        }  


        static void CadastrarProduto(){
            try
            {
                string codigoproduto;
                bool produtovalido;

                do{                   
                    Console.WriteLine("Digite o código do Produto");
                    codigoproduto = Console.ReadLine();
                    
                    produtovalido = VerificaProdutoCadastrado(codigoproduto);

                    if(!produtovalido)
                        Console.WriteLine("Código produto já cadastrado!");

                }while(produtovalido);

                Console.WriteLine("Digite o nome do produto");
                string nome = Console.ReadLine();

                Console.WriteLine("Digite a descrição do produto");
                string descricao = Console.ReadLine();

                Console.WriteLine("Digite o preço do produto");
                decimal preco = Convert.ToDecimal(Console.ReadLine());

                StreamWriter sr = new StreamWriter("produtos.txt", true);
                sr.WriteLine(codigoproduto + ";" + nome + ";" + descricao + ";" + preco);
                sr.Close();

                Console.WriteLine(" Produto " + nome + " cadastrado");
            }
            catch (Exception e)
            {
                GravarErro("CadastrarProduto", e.Message);
            } 
        }

        static void RealizarVenda(){
            string opcaopfpj = "";

                do{
                    
                    Console.WriteLine("Digite 1 para pessoa física  e 2 para pessoa jurídica");
                    opcaopfpj = Console.ReadLine();
                    if(opcaopfpj != "1" && opcaopfpj != "2"){
                        Console.WriteLine("Opção invalida");
                    }

                }while(opcaopfpj != "1" && opcaopfpj != "2");

                string documento;
                bool documentovalido = false;

                do{
                    if(opcaopfpj == "1"){
                        Console.WriteLine("Digite seu CPF");
                        documento = Console.ReadLine();
                        documentovalido = ValidarCPF(documento);

                        if(!documentovalido)
                            Console.WriteLine("CPF Inválido");
                    }
                    else{
                        Console.WriteLine("Digite seu CNPJ");
                        documento = Console.ReadLine();

                        documentovalido = ValidarCNPJ(documento);

                        if(!documentovalido)
                            Console.WriteLine("CNPJ Inválido");
    	            }
                }while(!documentovalido);

                bool clientecadastrado = VerificaClienteCadastrado(documento);

                if(!clientecadastrado)
                {
                    Console.WriteLine("Cliente não cadastrado, cadastre um novo cliente");
                    CadastrarCliente();
                }

                #region Busca dados Cliente
                    string[] clientes = File.ReadAllLines("clientes.txt");
                    string[] cliente = null;
                    foreach (var item in clientes)
                    {
                        cliente = item.Split(";");
                        if(cliente[0] == documento)
                        {
                            Console.WriteLine("Documento: " + cliente[0]);
                            Console.WriteLine("Nome: " + cliente[1]);
                            Console.WriteLine("Email: " + cliente[2]);
                            break;
                        }
                    }
                #endregion

                #region Lista Produtos
                    string[] produtos = File.ReadAllLines("produtos.txt");
                    string[] produto = null;
                    foreach (var item in produtos)
                    {
                        produto = item.Split(";");
                        Console.WriteLine(produto[0].PadRight(15) + produto[1].PadRight(25) + produto[2].PadRight(35) + produto[3].PadRight(20));
                    }
                #endregion

                string codigoproduto;
                bool produtoencontrado = false;

                do{
                    Console.WriteLine("Digite o código do produto");
                    codigoproduto  = Console.ReadLine();

                    produtoencontrado = VerificaProdutoCadastrado(codigoproduto);

                    if(!produtoencontrado)
                        Console.WriteLine("Código não encontrado, informe um código válido");

                }while(!produtoencontrado);

                #region Encontra produto
                   foreach (var item in produtos)
                    {
                        produto = item.Split(";");
                        if(produto[0] == codigoproduto){
                            Console.WriteLine("Produto escolhido " + produto[0].PadRight(15) + produto[1].PadRight(25) + produto[2].PadRight(35) + produto[3].PadRight(20));
                            break;
                        }
                        
                    }
                #endregion

                StreamWriter sw = new StreamWriter("vendas.txt", true);
                sw.WriteLine(cliente[0] + ";" + cliente[1] + ";" + produto[0]+ ";" + produto[1]+ ";" + produto[2]+ ";" + produto[3] );
                sw.Close();
        }

        static void ExtratoCliente(){
            String opcaopfpj = "";

             do{
                    
                    Console.WriteLine("Digite 1 para pessoa física  e 2 para pessoa jurídica");
                    opcaopfpj = Console.ReadLine();
                    if(opcaopfpj != "1" && opcaopfpj != "2"){
                        Console.WriteLine("Opção invalida");
                    }

                }while(opcaopfpj != "1" && opcaopfpj != "2");

                string documento;
                bool documentovalido = false;

                do{
                    if(opcaopfpj == "1"){
                        Console.WriteLine("Digite seu CPF");
                        documento = Console.ReadLine();
                        documentovalido = ValidarCPF(documento);

                        if(!documentovalido)
                            Console.WriteLine("CPF Inválido");
                    }
                    else{
                        Console.WriteLine("Digite seu CNPJ");
                        documento = Console.ReadLine();

                        documentovalido = ValidarCNPJ(documento);

                        if(!documentovalido)
                            Console.WriteLine("CNPJ Inválido");
    	            }
                }while(!documentovalido);

                if (!File.Exists("vendas.txt"))
                {
                    Console.WriteLine("Não foram efetuadas vendas!!!");
                }
                else
                {
                    string[] vendas = File.ReadAllLines("vendas.txt");
                    string[] arrayvenda;
                    foreach (var produto in vendas)
                    {
                        arrayvenda = produto.Split(";");
                        if (arrayvenda[0] == documento)
                        {
                            Console.WriteLine(arrayvenda[0].PadRight(15) + arrayvenda[1].PadRight(15) + arrayvenda[2].PadRight(25) + arrayvenda[4].PadRight(25));
                        }
                    }
                }

                
        }
        
        static void GravarErro(string funcao, string erro){
                try
                {
                    Console.WriteLine("Ocorreu um erro - Contacte o Administrador");
                    StreamWriter sr = new StreamWriter("logerro.txt", true);
                    sr.WriteLine(DateTime.Now + " - " + funcao + " - " + erro );
                    sr.Close();
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Ocorreu um erro - Contacte o Administrador");
                }
        }
    }
}
